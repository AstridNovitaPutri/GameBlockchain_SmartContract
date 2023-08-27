using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour
{
    public PlantObject plant;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI stockTxt;
    public TextMeshProUGUI priceTxt;
    public Image icon;
    public int sum;
    public int price;

    private void Start()
    {
        //initializeUI();
    }

    public void SetPlantSum(PlantObject _plant, int _sum, int _price)
    {
        plant = _plant;
        sum = _sum;
        price = _price;
        initializeUI();
    }

    void initializeUI()
    {
        nameTxt.text = plant.plantName;
        stockTxt.text = "Sold : " + sum + " x";
        priceTxt.text = "Total Price : Rp" + price;
        icon.sprite = plant.icon;
    }
}

