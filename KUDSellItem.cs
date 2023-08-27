using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KUDSellItem : UI_Drag
{
    public KUDStock stock;
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public Image[] icon;
    public TMP_InputField price;
    public TMP_InputField stockInput;
    public TextMeshProUGUI sumPrice;

    KUDStockManager kstockm;
    KUDShopManager kshop;

    private void Awake()
    {
        kstockm = FindObjectOfType<KUDStockManager>();
        kshop = FindObjectOfType<KUDShopManager>();
        //initializeUI();
    }

    public void UpdateSumPrice()
    {
        if (stockInput.text != null && price.text != null)
        {
            int totalPrice = (int.Parse(stockInput.text) * int.Parse(price.text));
            sumPrice.text = "Total : " + totalPrice.ToString();
        }
    }

    public void SetStock(KUDStock _stock, PlantObject _plant){
        stock = _stock;
        plant = _plant;
        initializeUI();
    }

    void initializeUI()
    {
        nameTxt.text = "Jual Tanaman " + stock.plantName;
        icon[0].sprite = plant.icon;
        icon[1].sprite = plant.icon;
        stockTxt.text = "/ " + stock.quantity;
        price.text = 0.ToString();
    }

    public void buy()
    {
        // string fileName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/KUD Shop Inventory/" + plant.plantName + "_" + DateTime.Now.ToString(@"dd\_MM\_yyyy") + ".asset");
        // int tempStock = Math.Abs(stock.quantity - int.Parse(stockInput.text));


        // if (stock.quantity < tempStock)
        // {
        //     Debug.Log("Not Enough Stock");
        // }
        // else if (int.Parse(stockInput.text) == 0 || stockInput.text == null)
        // {
        //     Debug.Log("Please put how many stock you want to sell");
        // }
        // else
        // {
        //     int tempAmount = stock.amount -= int.Parse(stockInput.text);

        //     stock.amount = tempAmount;
        //     EditorUtility.SetDirty(stock);

        //     KUDShopInventory KSI = ScriptableObject.CreateInstance<KUDShopInventory>();
        //     KSI.plant = stock.plant;
        //     KSI.owner = stock.owner;
        //     KSI.harvestedDate = stock.harvestedDate;
        //     KSI.expirationDate = stock.expirationDate;
        //     KSI.condition = stock.condition;
        //     KSI.amount = int.Parse(stockInput.text);
        //     KSI.price = int.Parse(price.text);

        //     AssetDatabase.CreateAsset(KSI, fileName);
        //     AssetDatabase.SaveAssets();
        //     AssetDatabase.Refresh();

        //     if (tempAmount == 0)
        //         DeleteAsset();

        //     initializeUI();
        //     kstockm.updateStock();
        //     kshop.updateShop();
        // }
        int stockToSold = int.Parse(stockInput.text); 
        if (stock.quantity < stockToSold){
            Debug.Log("Not Enough Stock");
        }else if (int.Parse(stockInput.text) == 0 || stockInput.text == null){
            Debug.Log("Please put how many stock you want to sell");
        }else{
            //int tempAmount = stock.totalHarvested -= int.Parse(stockInput.text);            
            KUDStockController.SellStock(stock, int.Parse(stockInput.text), int.Parse(price.text));
            Destroy(this.gameObject);
        }
        kstockm.UpdateFromDB();
        kshop.UpdateFromDB();
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
