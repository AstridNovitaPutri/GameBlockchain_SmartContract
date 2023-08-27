using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    //public ShopInventory SI;
    public FarmerShop farmerShopItem;
    public KUDShop kudShopItem;
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public Image icon;
    public TextMeshProUGUI price;
    public TextMeshProUGUI owner;
    public TextMeshProUGUI HarvestDate;
    public TextMeshProUGUI ExpDate;
    public TMP_InputField stockInput;
    public TextMeshProUGUI sumPrice;
    public GameObject buyButton;

    private SmartContractCall smartContractCall;

    ShopManager sm;
    int totalPrice;
    private void Awake()
    {
        sm = FindObjectOfType<ShopManager>();
        
    }

    private void AssignButtonBuy()
    {
        var button = buyButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        if (farmerShopItem != null) button.onClick.AddListener(KUDBuy);
        if (kudShopItem != null) button.onClick.AddListener(CustomerBuy);
    }

    public void UpdateSumPrice()
    {
        if (stockInput.text != null && stockInput.text.Length > 0)
        {
            if(farmerShopItem != null)
                totalPrice = (int.Parse(stockInput.text) * farmerShopItem.price);
            else if(kudShopItem != null)
                totalPrice = (int.Parse(stockInput.text) * kudShopItem.plantPrice);

            sumPrice.text = "Sum : " + totalPrice.ToString();
        }
    }

    public void SetStockAndPlant(FarmerShop _shopItem, PlantObject _plant, SmartContractCall _smartContractCall)
    {
        farmerShopItem = _shopItem;
        plant = _plant;
        InitializeFarmerItemUI();
        this.smartContractCall = _smartContractCall;
    }

    public void SetStockAndPlant(KUDShop _shopItem, PlantObject _plant, SmartContractCall _smartContractCall)
    {
        kudShopItem = _shopItem;
        plant = _plant;
        InitializeKUDItemUI();
        this.smartContractCall = _smartContractCall;
    }

    async void InitializeFarmerItemUI()
    {
        Debug.Log("Bapak penjual " + farmerShopItem.farmerAddress);
        var user = await UserController.GetUserFromETHAddress(farmerShopItem.farmerAddress);
        nameTxt.text = plant.plantName + " (" + farmerShopItem.condition + ")";
        icon.sprite = plant.icon;
        price.text = "Price : Rp " + farmerShopItem.price.ToString();
        owner.text = "By : " + user.username;
        HarvestDate.text = "Harvest Date : " + farmerShopItem.harvestedDate.Date.ToShortDateString();
        ExpDate.text = "Exp Date : " + farmerShopItem.expirationDate.Date.ToShortDateString();

        buyButton.SetActive(!sm.isFarmer);
        stockInput.gameObject.SetActive(!sm.isFarmer);
        sumPrice.gameObject.SetActive(!sm.isFarmer);

        if (sm.isFarmer)
        {
            stockTxt.text = "" + farmerShopItem.quantity;
            stockTxt.transform.localPosition = new Vector3(-4.37f, 0.00020671f, 0);
        }
        else
        {
            stockTxt.text = "/ " + farmerShopItem.quantity;
            stockTxt.transform.localPosition = new Vector3(73.7f, 0.00020671f, 0);
        }

        AssignButtonBuy();
    }

    async void InitializeKUDItemUI()
    {
        Debug.Log("Bapak kud: " + kudShopItem.kudAddress);
        var user = await UserController.GetUserFromETHAddress(kudShopItem.kudAddress);
        nameTxt.text = plant.plantName + " (" + kudShopItem.condition + ")";
        icon.sprite = plant.icon;
        price.text = "Price : Rp " + kudShopItem.plantPrice.ToString();
        owner.text = "By : " + user.username;
        HarvestDate.text = "Harvest Date : " + kudShopItem.harvestedDate.Date.ToShortDateString();
        ExpDate.text = "Exp Date : " + kudShopItem.expirationDate.Date.ToShortDateString();

        buyButton.SetActive(!sm.isFarmer);
        stockInput.gameObject.SetActive(!sm.isFarmer);
        sumPrice.gameObject.SetActive(!sm.isFarmer);

        if (sm.isFarmer)
        {
            stockTxt.text = "" + kudShopItem.quantity;
            stockTxt.transform.localPosition = new Vector3(-4.37f, 0.00020671f, 0);
        }
        else
        {
            stockTxt.text = "/ " + kudShopItem.quantity;
            stockTxt.transform.localPosition = new Vector3(73.7f, 0.00020671f, 0);
        }

        AssignButtonBuy();
    }

    public async void KUDBuy()
    {
        buyButton.SetActive(false);

        int stockToSold = int.Parse(stockInput.text); 

        if (farmerShopItem.quantity < stockToSold)
        {
            buyButton.SetActive(true);
            Debug.Log("Not Enough Stock");
        }
        else if (int.Parse(stockInput.text) == 0 || stockInput.text == null)
        {
            buyButton.SetActive(true);
            Debug.Log("Please put how many stock you want to sell");
        }
        else
        {
            var user = await UserController.GetMyUser();
            var totalPrice = stockToSold * farmerShopItem.price;
            user.Money -= totalPrice;

            if (user.Money < 0)
            {
                buyButton.SetActive(true);
                Debug.Log("Not enough money!");
                return;
            }

            var farmer = await FarmerController.LoadFarmerByAddress(farmerShopItem.farmerAddress);
            farmer.ACL.PublicReadAccess = true;
            farmer.ACL.PublicWriteAccess = true;
            await farmer.SaveAsync();

            user.ACL.PublicReadAccess = true;
            user.ACL.PublicWriteAccess = true;

            // buy with matic
            var result = await smartContractCall.SendRawMatic(user.ethAddress, farmer.ethAddress, totalPrice);
            if (!result.isSuccess) return;

            // add farmer's money
            var farmerAccount = await UserController.GetUserFromETHAddress(farmerShopItem.farmerAddress);
            farmerAccount.Money += totalPrice;

            FarmerShopController.KUDBuyItem(farmerShopItem, stockToSold);
            TransactionController.CreateFarmerKUDTransaction(farmerShopItem, stockToSold, result);

            await farmerAccount.SaveAsync();
            user.authData.Clear();
            await user.SaveAsync();
            Destroy(this.gameObject);
        }

        sm.UpdateFromDB();
        FindObjectOfType<UserUI>().updateUI();
    }

    public async void CustomerBuy()
    {
        buyButton.SetActive(false);

        int stockToSold = int.Parse(stockInput.text);

        if (kudShopItem.quantity < stockToSold)
        {
            buyButton.SetActive(true);
            Debug.Log("Not Enough Stock");
        }
        else if (int.Parse(stockInput.text) == 0 || stockInput.text == null)
        {
            buyButton.SetActive(true);
            Debug.Log("Please put how many stock you want to sell");
        }
        else
        {
            var user = await UserController.GetMyUser();
            var totalPrice = stockToSold * kudShopItem.plantPrice;
            user.Money -= totalPrice;

            if (user.Money < 0)
            {
                buyButton.SetActive(true);
                Debug.Log("Not enough money!");
                return;
            }

            var kud = await KUDController.LoadKUDByAddress(kudShopItem.kudAddress);
            kud.ACL.PublicReadAccess = true;
            kud.ACL.PublicWriteAccess = true;
            await kud.SaveAsync();

            user.ACL.PublicReadAccess = true;
            user.ACL.PublicWriteAccess = true;

            // buy with matic
            var result = await smartContractCall.SendRawMatic(user.ethAddress, kud.ethAddress, totalPrice);
            if (!result.isSuccess) return;

            // add kud's money
            var kudAccount = await UserController.GetUserFromETHAddress(kudShopItem.kudAddress);
            kudAccount.Money += totalPrice;

            kudShopItem.customerAddress = user.ethAddress;

            // substract customer's money
            KUDShopController.CustomerBuy(kudShopItem, stockToSold);
            TransactionController.CreateKUDCustomerTransaction(kudShopItem, stockToSold, result);

            await kudAccount.SaveAsync();
            user.authData.Clear();
            await user.SaveAsync();

            sm.UpdateFromDB();
            FindObjectOfType<UserUI>().updateUI();

            Destroy(this.gameObject);
        }
    }


    public void DeleteAsset()
    {
        //string pathToDelete = AssetDatabase.GetAssetPath(SI);
        //AssetDatabase.DeleteAsset(pathToDelete);
        AssetDatabase.Refresh();
    }

    void CreateAssetKUDStock()
    {
        string fileName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/KUD Stock/" + plant.plantName + "_" + DateTime.Now.ToString(@"dd\_MM\_yyyy") + ".asset");
        string date = DateTime.Now.ToString(@"dd\/MM\/yyyy");

        KUDStocks kds = ScriptableObject.CreateInstance<KUDStocks>();
        kds.plant = plant;
        kds.condition = farmerShopItem.condition;
        kds.harvestedDate = farmerShopItem.harvestedDate;
        kds.amount = int.Parse(stockInput.text);
        kds.boughtDate = date;
        kds.condition = farmerShopItem.condition;
        kds.owner = PlayerManager.instance.playerName;
        kds.boughtPrice = totalPrice;
        kds.expirationDate = farmerShopItem.expirationDate;

        AssetDatabase.CreateAsset(kds, fileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
