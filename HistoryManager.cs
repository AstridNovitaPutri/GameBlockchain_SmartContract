using System;
using System.Linq;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public GameObject Prefab;
    public bool isActive = false;
    public Transform stockP;
    public GameObject DatePicker;
    public bool refresh = false;

    public static DateTime from, to;

    private void Awake()
    {
        //updateStock();
        UpdateFromDB();
        //FarmerStockController.TestFarmerStock();
    }

    async void LoadData()
    {
        var datas = await FarmerHistoryController.GetMyHistories();

        Debug.Log(from + " " + to);

        var dataByDate = datas
            .Select(d => new { d.Key, d.Value })
            .Where(x => BetweenDate(x.Key.boughtTime, from, to));
        //.Where(x => x.Key.boughtTime >= from && x.Key.boughtTime <= to);

        var result = dataByDate
            .GroupBy(x => new { x.Value })
            .Select(g => new
            {
                Plant = g.Key.Value,
                Quantity = g.Sum(b => b.Key.quantity),
                Price = g.Sum(c => c.Key.totalPrice),
            });

        //var dataByDate = datas
        //    .Select(d => new { d.Key, d.Value })
        //    .Where(x => BetweenDate(x.Key.harvestedDate, from, to));
        //var result = dataByDate
        //    .GroupBy(x => new { x.Value })
        //    .Select(g => new { Plant = g.Key.Value, Sum = g.Sum(b => b.Key.totalHarvested) });
        foreach (var i in result)
        {
            HistoryItem newData = Instantiate(Prefab, transform).GetComponent<HistoryItem>();
            newData.SetPlantSum(i.Plant, i.Quantity, i.Price);
        }
    }

    bool BetweenDate(DateTime n, DateTime from, DateTime to)
    {
        return (n >= from && n <= to);
    }

    public void InstantiateDatePicker(bool isFrom)
    {
        var instance = Instantiate(DatePicker, transform.parent.parent.parent).GetComponentInChildren<DatePicker>();
        instance.isFrom = isFrom;
    }

    public void UpdateFromDB()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Invoke("LoadData", 0.5f);
    }
    public void CloseButton()
    {
        transform.parent.parent.parent.gameObject.SetActive(false);
    }
}
