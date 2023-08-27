using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KUDStockItem : MonoBehaviour
{
    public KUDStock stock;
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI owner;
    public TextMeshProUGUI stockTxt;
    public TextMeshProUGUI harvestDate;
    public TextMeshProUGUI expDate;
    public TextMeshProUGUI boughtDate;
    public TextMeshProUGUI boughtPrice;
    public TextMeshProUGUI condition;
    public Image icon;
    public GameObject sellItem;

    private void Start()
    {
        //initializeUI();
    }

    public void SetStockAndPlant(KUDStock _stock, PlantObject _plant){
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
        boughtDate.text = "Bought Date : " + stock.boughtTime.Date.ToShortDateString();
        boughtPrice.text = "Bought Price : " + stock.plantPrice;
        condition.text = "Condition : " + stock.condition;
        icon.sprite = plant.icon;
    }

    public void Sell()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        KUDSellItem ksi = sellItem.GetComponent<KUDSellItem>();
        KUDSellItem ksiInstantiate = Instantiate(ksi, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity, canvasObj.transform);
        ksiInstantiate.SetStock(stock, plant);
    }
}
