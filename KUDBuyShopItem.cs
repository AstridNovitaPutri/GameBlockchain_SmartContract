using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KUDBuyShopItem : UI_Drag
{
    public KUDShopInventory stock;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public Image[] icon;
    public TextMeshProUGUI price;
    public TMP_InputField stockInput;
    public TextMeshProUGUI sumPrice;

    KUDShopManager kshop;

    private void Awake()
    {
        kshop = FindObjectOfType<KUDShopManager>();
        initializeUI();
    }

    public void UpdateSumPrice()
    {
        if (stockInput.text != null)
        {
            int totalPrice = (int.Parse(stockInput.text) * int.Parse(price.text));
            sumPrice.text = "Total : " + totalPrice.ToString();
        }
    }

    void initializeUI()
    {
        nameTxt.text = "Beli Tanaman " + stock.plant.plantName;
        icon[0].sprite = stock.plant.icon;
        icon[1].sprite = stock.plant.icon;
        price.text = stock.price.ToString();
    }

    public void buy()
    {
        string fileName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Konsumen Stock/" + stock.plant.plantName + "_" + DateTime.Now.ToString(@"dd\_MM\_yyyy") + ".asset");
        int tempStock = Math.Abs(stock.amount - int.Parse(stockInput.text));
        DateTime date = DateTime.Now;

        if (stock.amount < tempStock)
        {
            Debug.Log("Not Enough Stock");
        }
        else if (int.Parse(stockInput.text) == 0 || stockInput.text == null)
        {
            Debug.Log("Please put how many stock you want to sell");
        }
        else
        {
            int tempAmount = stock.amount -= int.Parse(stockInput.text);

            stock.amount = tempAmount;
            EditorUtility.SetDirty(stock);

            KonsumenStock KSI = ScriptableObject.CreateInstance<KonsumenStock>();
            KSI.plant = stock.plant;
            KSI.owner = stock.owner;
            KSI.konsumen = PlayerManager.instance.playerName;
            KSI.harvestedDate = stock.harvestedDate;
            KSI.condition = stock.condition;
            KSI.amount = int.Parse(stockInput.text);
            KSI.expirationDate = stock.expirationDate;
            KSI.boughtDate = date;
            KSI.boughtPrice = int.Parse(price.text);
            KSI.status = KonsumenStock.Status.requested;

            AssetDatabase.CreateAsset(KSI, fileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (tempAmount == 0)
                DeleteAsset();

            initializeUI();
            kshop.updateShop();
        }
    }

    public void DeleteAsset()
    {
        string pathToDelete = AssetDatabase.GetAssetPath(stock);
        AssetDatabase.DeleteAsset(pathToDelete);
        AssetDatabase.Refresh();
    }

    public void cancel()
    {
        Destroy(this.gameObject);
    }
}
