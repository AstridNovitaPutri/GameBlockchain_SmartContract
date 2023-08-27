using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestItem : MonoBehaviour
{
    public KonsumenStock stock;
    public CustomerStock customerStock;
    public PlantObject plantObject;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI owner;
    public TextMeshProUGUI stockTxt;
    public TextMeshProUGUI harvestDate;
    public TextMeshProUGUI expDate;
    public TextMeshProUGUI condition;
    public TextMeshProUGUI price;
    public TextMeshProUGUI status;
    public TextMeshProUGUI konsumen;
    public TextMeshProUGUI distributor;
    public Image icon;
    public GameObject acceptButton;
    public GameObject[] konsumenButton;

    RequestManager ksm;

    private void Start()
    {
        ksm = FindObjectOfType<RequestManager>();
    }

    public void InitializeItem(CustomerStock customerStock, PlantObject plantObject)
    {
        this.customerStock = customerStock;
        this.plantObject = plantObject;
        initializeUI();
    }

    protected void initializeUI()
    {
        nameTxt.text = customerStock.plantName;
        owner.text = "Owner : " + customerStock.kudAddress;
        stockTxt.text = "Amount : " + customerStock.quantity;
        harvestDate.text = "Harvest Date : " + customerStock.harvestedDate;
        expDate.text = "Exp Date : " + customerStock.expirationDate;
        condition.text = "Condition : " + customerStock.condition;
        icon.sprite = plantObject.icon;
        price.text = "Price : " + customerStock.plantPrice;
        status.text = "Status : " + customerStock.GetStatus();
        konsumen.text = "Konsumen : " + customerStock.customerAddress;
        distributor.text = "Distributor : " + (string.IsNullOrEmpty(customerStock.distributorAddress) ? "-" : customerStock.distributorAddress);

        //acceptButton.SetActive(!ksm.isKonsumen && stock.status == KonsumenStock.Status.requested);
        //konsumenButton[0].SetActive(ksm.isKonsumen && stock.status == KonsumenStock.Status.arrived);
        //konsumenButton[1].SetActive(ksm.isKonsumen && stock.status == KonsumenStock.Status.rejected);
    }

    public void RequestSent()
    {
        stock.status = KonsumenStock.Status.sent;
        stock.distributor = PlayerManager.instance.playerName;
        stock.owner = PlayerManager.instance.playerName;
        initializeUI();
        ksm.updateShop();
    }

    public void RequestAccepted()
    {
        stock.status = KonsumenStock.Status.received;
        stock.owner = PlayerManager.instance.playerName;
        initializeUI();
        ksm.updateShop();
    }

    public void RequestRejected()
    {
        stock.status = KonsumenStock.Status.rejected;
        initializeUI();
        ksm.updateShop();
    }

    public void RequestArrived()
    {
        stock.status = KonsumenStock.Status.arrived;
        initializeUI();
        ksm.updateShop();
    }
}
