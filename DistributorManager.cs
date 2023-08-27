using MoralisUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistributorManager : MonoBehaviour
{
    public GameObject prefabItem;
    public GameObject requestSpawn;
    public GameObject completedSpawn;

    List<DistributorItem> requestList = new List<DistributorItem>();
    List<DistributorItem> completedList = new List<DistributorItem>();

    // Start is called before the first frame update
    void Start()
    {
        Moralis.Start();
        FetchItems();
    }

    public void RetryFetchItems()
    {
        ResetAllItems();
        FetchItems();
    }

    async void FetchItems()
    {
        var moralisUser = await Moralis.GetUserAsync();
        var stocks = await DistributorController.GetShopItems();
        stocks.OrderBy((x) => x.Key.harvestedDate);
        foreach (var stock in stocks)
        {
            DistributorItem item;

            if (stock.Key.IsCompleted())
            {
                if (stock.Key.distributorAddress != moralisUser.ethAddress) continue;

                item = Instantiate(prefabItem, completedSpawn.transform).GetComponent<DistributorItem>();
                completedList.Add(item);
            }
            else
            {
                item = Instantiate(prefabItem, requestSpawn.transform).GetComponent<DistributorItem>();
                requestList.Add(item);
            }

            item.InitializeItem(stock.Key, stock.Value, moralisUser.ethAddress);
        }
    }

    private void ResetAllItems()
    {
        foreach(var item in requestList)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in completedList)
        {
            Destroy(item.gameObject);
        }

        requestList.Clear();
        completedList.Clear();
    }
}
