using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Queries;
using System.Collections.Generic;
using UnityEngine;

public class FarmerHistoryController
{
    //private class FarmerHistory : FarmerHistoryModel { }

    public static async UniTask<Dictionary<FarmerKUDTransaction, PlantObject>> GetHistories(string _userAddress)
    {
        MoralisQuery<FarmerKUDTransaction> farmer = await Moralis.Query<FarmerKUDTransaction>();

        farmer = farmer.WhereEqualTo("farmerAddress", _userAddress);
        IEnumerable<FarmerKUDTransaction> result = await farmer.FindAsync();
        var list = new Dictionary<FarmerKUDTransaction, PlantObject>();

        Debug.Log(farmer);

        foreach (FarmerKUDTransaction item in result)
        {
            Debug.Log("Sukses");
            var plant = ScriptableUtils.GetPlant(item.plantName);
            list.Add(item, plant);
        }

        return list;
    }

    public static async UniTask<Dictionary<FarmerKUDTransaction, PlantObject>> GetMyHistories()
    {
        UserModel user = await UserController.GetMyUser();
        Debug.Log(user.ethAddress);
        return await GetHistories(user.ethAddress);
    }
}
