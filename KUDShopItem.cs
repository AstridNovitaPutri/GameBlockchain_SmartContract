using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KUDShopItem : MonoBehaviour
{
    public KUDShop stock;
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI owner;
    public TextMeshProUGUI stockTxt;
    public TextMeshProUGUI harvestDate;
    public TextMeshProUGUI expDate;
    public TextMeshProUGUI condition;
    public TextMeshProUGUI price;
    public Image icon;
    public GameObject sellItem;
    public GameObject sellButton;

    KUDShopManager ksm;

    private void Awake()
    {
        ksm = FindObjectOfType<KUDShopManager>();
        //initializeUI();
    }

    public void SetStockAndPlant(KUDShop _stock, PlantObject _plant)
    {
        stock = _stock;
        plant = _plant;
        initializeUI();
    }

    protected async void initializeUI()
    {
        var user = await UserController.GetUserFromETHAddress(stock.farmerAddress);
        nameTxt.text = plant.name;
        owner.text = "Owner : " + user.username;
        stockTxt.text = "Amount : " + stock.quantity;
        harvestDate.text = "Harvest Date : " + stock.harvestedDate.Date.ToShortDateString();
        expDate.text = "Exp Date : " + stock.expirationDate.Date.ToShortDateString();
        condition.text = "Condition : " + stock.condition;
        icon.sprite = plant.icon;
        price.text = "Price : " + stock.plantPrice;
        sellButton.SetActive(!ksm.isKUD);
    }

    public void Sell()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        KUDBuyShopItem ksi = sellItem.GetComponent<KUDBuyShopItem>();
        //ksi.stock = stock;
        //Debug.Log(stock);
        Instantiate(ksi, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity, canvasObj.transform);
    }
}
