using System.Collections;
using UnityEngine;

public class StockManager : MonoBehaviour
{
    public GameObject stockPrefab;
    public bool isActive = false;
    public Transform stockP;

    private void Awake()
    {
        //updateStock();
        UpdateFromDB();
        //FarmerStockController.TestFarmerStock();
    }

    public void updateStock()
    {
        var loadStocks = Resources.LoadAll("Stock", typeof(Stocks));

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var stock in loadStocks)
        {
            StockItem newStock = Instantiate(stockPrefab, transform).GetComponent<StockItem>();
            //newStock.stock = (FarmerStockModel)stock;
        }
    }

    public void UpdateFromDB()
    {
        Debug.Log("Farmer Stock Initiated");
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Invoke("DelayQuery", 0.5f);
    }

    async void DelayQuery()
    {
        var stocks = await FarmerStockController.GetMyStocks();
        foreach (var stock in stocks)
        {
            Debug.Log("New Stock Instantiated " + stock.Key.totalHarvested);
            StockItem newStock = Instantiate(stockPrefab, transform).GetComponent<StockItem>();
            newStock.SetStockAndPlant((FarmerStock)stock.Key, stock.Value);
        }
    }

    public void active()
    {
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 target = new Vector3(-914, 0, 0);
        if (isActive)
        {
            StartCoroutine(Move(start, target));
        }
        else
        {
            StartCoroutine(Move(target, start));
        }
        isActive = !isActive;
    }

    IEnumerator Move(Vector3 start, Vector3 target)
    {
        float time = 0.0f;

        while (time < 0.5)
        {
            time += Time.deltaTime;
            stockP.localPosition = Vector3.Lerp(start, target, time / 0.5f);
            yield return null;
        }
    }
}
