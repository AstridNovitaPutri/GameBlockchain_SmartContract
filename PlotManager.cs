using System;
using UnityEditor;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public bool isPlanted = false;
    public bool isRipe = false;

    GameObject plant;

    int plantStage;
    float timer;

    PlantObject selectedPlant;

    PlantManager pm;
    StockManager sm;
    ListManager lm;

    private void Start()
    {
        pm = transform.parent.GetComponent<PlantManager>();
        sm = FindObjectOfType<StockManager>();
        lm = FindObjectOfType<ListManager>();
    }

    private void Update()
    {
        if (isPlanted)
        {
            timer -= Time.deltaTime;

            if (plantStage == selectedPlant.plantStages.Length - 1)
            {
                isRipe = true;
            }

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                timer = (selectedPlant.growTime / selectedPlant.plantStages.Length);
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (isRipe)
                //Harvest();
                HarvestDB();
        }
        else if (pm.isPlanting)
        {
            Plant(pm.selectPlant.plant);
        }
    }

    void Harvest()
    {
        Stocks tempStock, newStock;
        var loadStocks = Resources.LoadAll("Stock", typeof(Stocks));
        DateTime date = DateTime.Now;
        DateTime expDate = DateTime.Now.AddDays(selectedPlant.lifeExpetancy);
        string fileName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Stock/" + selectedPlant.plantName + "_" + DateTime.Now.ToString(@"dd\_MM\_yyyy") + ".asset");

        bool stockAlreadyExist = false;

        for (int i = 0; i < loadStocks.Length; i++)
        {
            tempStock = (Stocks)loadStocks[i];
            if (tempStock.harvestedDate == date && tempStock.plant == selectedPlant)
            {
                Debug.Log("Existed");
                tempStock.totalHarvested++;
                stockAlreadyExist = true;
                if (!EditorUtility.IsDirty(tempStock))
                {
                    EditorUtility.SetDirty(tempStock);
                }
                break;
            }
        }

        if (!stockAlreadyExist)
        {
            newStock = ScriptableObject.CreateInstance<Stocks>();
            newStock.harvestedDate = date;
            newStock.expirationDate = expDate;
            newStock.condition = "Fresh";
            newStock.plant = selectedPlant;
            newStock.farmer = PlayerManager.instance.playerName;
            newStock.totalHarvested += 1;
            Debug.Log("Create New Stock");
            AssetDatabase.CreateAsset(newStock, fileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        isPlanted = false;
        sm.updateStock();
        Destroy(plant);
    }

    async void HarvestDB()
    {
        var user = await UserController.GetMyUser();
        FarmerStock farmerStockModel = new FarmerStock();
        farmerStockModel.farmerAddress = user.ethAddress;
        farmerStockModel.plantName = selectedPlant.plantName;
        farmerStockModel.condition = "Fresh";
        farmerStockModel.harvestedDate = DateTime.Now.ToUniversalTime();
        farmerStockModel.expirationDate = DateTime.Now.ToUniversalTime().Date.AddDays(selectedPlant.lifeExpetancy);
        var stock = await FarmerStockController.GetMyStocks();
        FarmerStockController.AddFarmerStock(farmerStockModel, 1);
        sm.UpdateFromDB();
        Destroy(plant.gameObject);
        isPlanted = false;
    }

    public void Plant(PlantObject newPlant)
    {
        //Debug.Log("Planted");
        if (!sm.isActive && !lm.isActive)
        {
            selectedPlant = newPlant;
            isPlanted = true;
            plantStage = 0;
            UpdatePlant();
            isRipe = false;
            timer = (selectedPlant.growTime / selectedPlant.plantStages.Length);
        }
    }

    void UpdatePlant()
    {
        plant = (GameObject)Instantiate(selectedPlant.plantStages[plantStage], transform.position, Quaternion.identity, this.transform);
        if (plantStage != selectedPlant.plantStages.Length - 1)
        {
            Destroy(plant, (selectedPlant.growTime / selectedPlant.plantStages.Length));
        }
    }
    public bool isPlantRipe()
    {
        return isRipe;
    }
}
