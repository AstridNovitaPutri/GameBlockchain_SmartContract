using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerItem : MonoBehaviour
{
    private CustomerStock customerStock;
    private PlantObject plant;
    public Image icon;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI farmerTxt;
    public TextMeshProUGUI kudTxt;
    public TextMeshProUGUI distributorTxt;
    public TextMeshProUGUI customerTxt;
    public TextMeshProUGUI sendDateTxt;
    public TextMeshProUGUI receivedDateTxt;
    public TextMeshProUGUI statusTxt;
    public TextMeshProUGUI quantityTxt;
    public TextMeshProUGUI harvestDate;
    public TextMeshProUGUI expDate;
    public TextMeshProUGUI price;

    public void InitializeItem(CustomerStock customerStock, PlantObject plant)
    {
        this.customerStock = customerStock;
        this.plant = plant;

        icon.sprite = plant.icon;
        nameTxt.text = customerStock.plantName + string.Format(" ({0})", customerStock.condition);
        farmerTxt.text = "Farmer: " + customerStock.farmerAddress;
        kudTxt.text = "KUD: " + customerStock.kudAddress;
        distributorTxt.text = "Distributor: " + (string.IsNullOrEmpty(customerStock.distributorAddress) ? "-" : customerStock.distributorAddress);
        customerTxt.text = "Customer: " + customerStock.customerAddress;
        statusTxt.text = "Status: " + customerStock.GetStatus();
        quantityTxt.text = "Quantity: " + customerStock.quantity.ToString();
        harvestDate.text = "Harvest Date: " + customerStock.harvestedDate;
        expDate.text = "Exp Date: " + customerStock.expirationDate.ToString();
        sendDateTxt.text = "Send Date: " + (IsDefaultDateTime(customerStock.sendDate) ? "-" : customerStock.sendDate.ToString());
        receivedDateTxt.text = "Recieved Date: " + (IsDefaultDateTime(customerStock.receivedDate) ? "-" : customerStock.receivedDate.ToString());
        price.text = "Price: " + customerStock.plantPrice;
    }

    private bool IsDefaultDateTime(DateTime dateTime)
    {
        return (dateTime == DateTime.MinValue || dateTime == new DateTime(1970, 1, 1));
    }
}
