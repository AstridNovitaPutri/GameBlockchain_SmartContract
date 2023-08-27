using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorController
{
    public static async UniTask<Dictionary<CustomerStock, PlantObject>> GetShopItems()
    {
        MoralisQuery<CustomerStock> query = await Moralis.Query<CustomerStock>();
        IEnumerable<CustomerStock> result = await query.FindAsync();
        var list = new Dictionary<CustomerStock, PlantObject>();

        foreach (CustomerStock item in result)
        {
            var plant = ScriptableUtils.GetPlant(item.plantName);
            list.Add(item, plant);
        }

        return list;
    }

    public static async void DistributorSendItem(DistributorItem distributorItem, Action onCompleted = null)
    {
        MoralisQuery<CustomerStock> query = await Moralis.Query<CustomerStock>();
        var result = await query.FindAsync();

        foreach (CustomerStock item in result)
        {
            if(item.objectId == distributorItem.CustomerStock.objectId)
            {
                item.distributorAddress = distributorItem.CustomerStock.distributorAddress;
                item.sendDate = DateTime.Now.ToUniversalTime();

                // random between 1 - 30 days
                int randAddDays = UnityEngine.Random.Range(1, 30);
                item.receivedDate = DateTime.Now.ToUniversalTime().AddDays(randAddDays);
                item.status = 2;
                await item.SaveAsync();
                Debug.Log("Updated status successfully.");
                onCompleted?.Invoke();
            }
        }
    }
}
