using MoralisUnity;
using UnityEngine;

public class KUDShopManager : MonoBehaviour
{
    public GameObject KUDShopPrefab;
    public bool isKUD = true;

    private void Awake()
    {
        //isKUD = PlayerManager.instance.playerType == "KUD";
        //updateShop();
    }

    private async void OnEnable()
    {
        Moralis.Start();
        var user = await Moralis.GetUserAsync();
        var farmer = await UserController.GetUser(user.objectId);
        isKUD = farmer.Role == "KUD";
        //Debug.Log("Farmer " + isKUD);
        //updateShop();
        UpdateFromDB();
    }



    public void updateShop()
    {
        var loadShop = Resources.LoadAll("KUD Shop Inventory", typeof(KUDShopInventory));

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KUDShopInventory shop in loadShop)
        {
            if (isKUD)
            {
                if (shop.owner == PlayerManager.instance.playerName)
                {
                    KUDShopItem newShop = Instantiate(KUDShopPrefab, transform).GetComponent<KUDShopItem>();
                    //newShop.stock = shop;
                }
            }
            else
            {
                KUDShopItem newShop = Instantiate(KUDShopPrefab, transform).GetComponent<KUDShopItem>();
                //newShop.stock = shop;
            }
        }
    }

    public void UpdateFromDB(){
        Debug.Log("KUD Shop Initiated");
        foreach (Transform child in transform){
            GameObject.Destroy(child.gameObject);
        }
        Invoke("DelayQuery", 0.5f);
    }

    async void DelayQuery(){
        var shops = await KUDShopController.GetMyStocks();
        foreach (var shop in shops){
            KUDShopItem newStock = Instantiate(KUDShopPrefab, transform).GetComponent<KUDShopItem>();
            newStock.SetStockAndPlant((KUDShop)shop.Key, shop.Value);
        }
    }
}
