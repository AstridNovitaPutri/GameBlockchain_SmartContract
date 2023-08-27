using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SellItem : UI_Drag
{
    public FarmerStock stock;
    private PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public Image[] icon;
    public TMP_InputField price;
    public TMP_InputField stockInput;
    public TextMeshProUGUI sumPrice;

    StockManager sm;

    private void Awake()
    {
        sm = FindObjectOfType<StockManager>();
        //initializeUI();
    }

    public void SetStock(FarmerStock _stock, PlantObject _plant)
    {
        stock = _stock;
        plant = _plant;
        initializeUI();
    }

    public void UpdateSumPrice()
    {
        if (stockInput.text != null && price.text != null)
        {
            int totalPrice = (int.Parse(stockInput.text) * int.Parse(price.text));
            sumPrice.text = "Total : " + totalPrice.ToString();
        }
    }

    void initializeUI()
    {
        nameTxt.text = "Jual Tanaman " + plant.plantName;
        icon[0].sprite = plant.icon;
        icon[1].sprite = plant.icon;
        Debug.Log("Stock in SellItem : " + (stock == null));
        stockTxt.text = "/ " + stock.totalHarvested;
        price.text = 0.ToString();
    }

    public void buy()
    {
        int stockToSold = int.Parse(stockInput.text);
        if (stock.totalHarvested < stockToSold)
        {
            Debug.Log("Not Enough Stock");
        }
        else if (int.Parse(stockInput.text) == 0 || stockInput.text == null)
        {
            Debug.Log("Please put how many stock you want to sell");
        }
        else
        {
            //int tempAmount = stock.totalHarvested -= int.Parse(stockInput.text);            
            FarmerStockController.SellStock(stock, int.Parse(stockInput.text), int.Parse(price.text));
            Destroy(this.gameObject);
        }
        sm.UpdateFromDB();
    }

    public void DeleteAsset()
    {
        //string pathToDelete = AssetDatabase.GetAssetPath(stock);
        //AssetDatabase.DeleteAsset(pathToDelete);
        AssetDatabase.Refresh();
    }

    public void cancel()
    {
        Destroy(this.gameObject);
    }
}
