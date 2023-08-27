using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DistributorItem : MonoBehaviour
{
    private CustomerStock customerStock;
    private PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public Image icon;
    public TextMeshProUGUI customer;
    public TextMeshProUGUI status;
    public Button buttonAccept;
    public CustomerStock CustomerStock { get => customerStock; }

    public void InitializeItem(CustomerStock customerStock, PlantObject plant, string distributorAddress)
    {
        this.customerStock = customerStock;
        this.customerStock.distributorAddress = distributorAddress;
        this.plant = plant;

        nameTxt.text = customerStock.plantName;
        stockTxt.text = "Quantity: " + customerStock.quantity.ToString();
        icon.sprite = plant.icon;
        customer.text = "Customer: " + customerStock.customerAddress;
        status.text = "Status: " + customerStock.GetStatus();

        buttonAccept.gameObject.SetActive(!customerStock.IsCompleted());
        buttonAccept.onClick.AddListener(() =>
        {
            buttonAccept.gameObject.SetActive(false);
            DistributorController.DistributorSendItem(this, onCompleted: () => SceneManager.LoadScene("Cutscene"));
        });
    }
}
