using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject spawnTarget;
    public bool isKonsumen = true;
    private List<CustomerItem> customerItemList = new List<CustomerItem>();

    private void Awake()
    {
        Moralis.Start();
        //updateShop();
        //RequestCustomerItems();
        FetchItems();
    }

    public void RetryFetchItems()
    {
        ResetAllItems();
        FetchItems();
    }

    public void updateShop()
    {
        var loadItem = Resources.LoadAll("Konsumen Stock", typeof(KonsumenStock));

        if(transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Don't have child");
        }

        if (loadItem != null && loadItem.Length > 0)
        {
            foreach (KonsumenStock item in loadItem)
            {
                if(PlayerManager.instance != null)
                {
                    if (item.status != KonsumenStock.Status.rejected && item.konsumen == PlayerManager.instance.playerName)
                    {
                        RequestItem newItem = Instantiate(itemPrefab, transform).GetComponent<RequestItem>();
                        newItem.stock = item;
                    }
                }
                else
                {
                    Debug.LogWarning("PlayerManager Instance is null");
                }
            }
        }
        else
        {
            Debug.LogWarning("Don't have load item");
        }
    }

    async void RequestCustomerItems()
    {
        var stocks = await CustomerStockController.GetMyStocks();
        foreach (var stock in stocks)
        {
            RequestItem newItem = Instantiate(itemPrefab, transform).GetComponent<RequestItem>();
            newItem.InitializeItem(stock.Key, stock.Value);
        }
    }

    private async void FetchItems()
    {
        var stocks = await CustomerStockController.GetMyStocks();
        var sortedStocks = stocks.OrderByDescending(x => x.Key.harvestedDate);
        foreach (var stock in sortedStocks)
        {
            CustomerItem newItem = Instantiate(itemPrefab, spawnTarget.transform).GetComponent<CustomerItem>();
            newItem.InitializeItem(stock.Key, stock.Value);
            customerItemList.Add(newItem);
        }
    }

    private void ResetAllItems()
    {
        foreach(var item in customerItemList)
        {
            Destroy(item.gameObject);
        }

        customerItemList.Clear();
    }

    [ContextMenu("TestTransaction")]
    public void TestTransaction()
    {
        SendRawETH();
    }

    private async void SendRawETH()
    {
        string toAddress = "0x10e255f21ca79a1a80cf3f70cf989589c0a0269d";
        float transferAmount = .02f;
        try
        {
            string txnHash = await Moralis.SendTransactionAsync(toAddress, new HexBigInteger(UnitConversion.Convert.ToWei(transferAmount)));
            Debug.Log($"Transfered {transferAmount} ETH to {toAddress}.  TxnHash: {txnHash}");
        }
        catch (Exception exp)
        {
            Debug.Log($"Transfer of {transferAmount} ETH to {toAddress} failed! with error {exp}");
        }
    }
}
