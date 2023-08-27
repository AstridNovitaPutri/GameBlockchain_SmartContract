using UnityEngine;

public class KUDStockManager : MonoBehaviour
{
    public GameObject KUDStockPrefab;

    private void Awake()
    {
        //updateStock();
        UpdateFromDB();
    }

    public void updateStock()
    {
        var loadStock = Resources.LoadAll("KUD Stock", typeof(KUDStocks));

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KUDStocks stock in loadStock)
        {
            if (stock.owner == PlayerManager.instance.playerName)
            {
                KUDStockItem newStock = Instantiate(KUDStockPrefab, transform).GetComponent<KUDStockItem>();
                //newStock.stock = stock;
            }
        }
    }

    public void UpdateFromDB(){
        Debug.Log("KUD Stock Initiated");
        foreach (Transform child in transform){
            GameObject.Destroy(child.gameObject);
        }
        Invoke("DelayQuery", 0.5f);
    }

    async void DelayQuery(){
        var stocks = await KUDStockController.GetMyStocks();
        foreach (var stock in stocks){
            KUDStockItem newStock = Instantiate(KUDStockPrefab, transform).GetComponent<KUDStockItem>();
            newStock.SetStockAndPlant((KUDStock)stock.Key, stock.Value);
        }
    }
}
