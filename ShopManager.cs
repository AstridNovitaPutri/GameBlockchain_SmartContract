using MoralisUnity;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public UserController.UserRole userRole;
    public SmartContractCall smartContractCall;

    public GameObject shopPrefab;
    public GameObject spawnParent;
    public TMP_Text titleText;
    public bool isFarmer = false;

    private void Awake()
    {
        //isFarmer = PlayerManager.instance.playerType == "Farmer";
        updateShop();
    }

    private void InitTitle()
    {
        if (userRole == UserController.UserRole.Distributor || userRole == UserController.UserRole.Farmer ||
            userRole == UserController.UserRole.KUD)
            titleText.text = "Penjualan Petani";
        else if (userRole == UserController.UserRole.Customer)
            titleText.text = "Penjualan KUD";
        else
            titleText.text = "-";
    }

    private async void OnEnable()
    {
        Moralis.Start();
        var moralisUser = await Moralis.GetUserAsync();
        var appUser = await UserController.GetUser(moralisUser.objectId);

        switch (appUser.Role)
        {
            case "Farmer":
                userRole = UserController.UserRole.Farmer;
                break;
            case "KUD":
                userRole = UserController.UserRole.KUD;
                break;
            case "Distributor":
                userRole = UserController.UserRole.Distributor;
                break;
            case "Customer":
                userRole = UserController.UserRole.Customer;
                break;
        }

        InitTitle();
        isFarmer = appUser.Role == "Farmer";
        Debug.Log("Farmer " + isFarmer);
        //updateShop();
        UpdateFromDB();
    }

    public void updateShop()
    {
        var loadShop = Resources.LoadAll("Shop Inventory", typeof(ShopInventory));

        foreach (Transform child in spawnParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ShopInventory shop in loadShop)
        {
            ShopItem newShop = Instantiate(shopPrefab, spawnParent.transform).GetComponent<ShopItem>();
            //newShop.SI = shop;
        }
    }

    public void UpdateFromDB() {
        Debug.Log("Farmer Stock Initiated");
        foreach (Transform child in spawnParent.transform) {
            GameObject.Destroy(child.gameObject);
        }

        if (userRole == UserController.UserRole.KUD || userRole == UserController.UserRole.Farmer)
            Invoke("DelayQuery", 0.5f);
        else
            Invoke("DelayQueryKUDItems", 0.5f);
    }

    async void DelayQuery(){
        var isFarmer = userRole == UserController.UserRole.Farmer ? true : false;
        var stocks = await FarmerShopController.GetShopItems(isFarmer);
        foreach (var stock in stocks){
            ShopItem newStock = Instantiate(shopPrefab, spawnParent.transform).GetComponent<ShopItem>();
            newStock.SetStockAndPlant(stock.Key, stock.Value, smartContractCall);
        }
    }

    async void DelayQueryKUDItems()
    {
        var isKUD = userRole == UserController.UserRole.KUD ? true : false;
        var stocks = await KUDShopController.GetShopItems(isKUD);
        foreach (var stock in stocks)
        {
            ShopItem newStock = Instantiate(shopPrefab, spawnParent.transform).GetComponent<ShopItem>();
            newStock.SetStockAndPlant(stock.Key, stock.Value, smartContractCall);
        }
    }
}
