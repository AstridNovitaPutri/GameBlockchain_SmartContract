using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockItem : MonoBehaviour
{
    public FarmerStock stock;
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public TextMeshProUGUI harvestDate;
    public TextMeshProUGUI expDate;
    public Image icon;
    public GameObject sellItem;

    private void Start()
    {
        //initializeUI();
    }

    public void SetStockAndPlant(FarmerStock _stock, PlantObject _plant)
    {
        stock = _stock;
        plant = _plant;
        initializeUI();
    }

    void initializeUI()
    {
        nameTxt.text = plant.plantName;
        stockTxt.text = stock.totalHarvested + " x";
        icon.sprite = plant.icon;
        harvestDate.text = "Harvest Date : " + stock.harvestedDate.Date.ToShortDateString();
        expDate.text = "Exp Date : " + stock.expirationDate.Date.ToShortDateString();
    }

    public void Sell()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        SellItem si = sellItem.GetComponent<SellItem>();
        //Debug.Log(plant.name);
        //Debug.Log(si.plant.name);
        SellItem siInstantiate = Instantiate(si, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity, canvasObj.transform);
        siInstantiate.SetStock(stock, plant);
    }
}

