using MoralisUnity;
using UnityEngine;

public class MoralisController : MonoBehaviour
{

    private static async void SaveToMoralisDatabase(Stocks _stock)
    {
        var moralisStok = Moralis.Client.Create<FarmerStock>();
        moralisStok.farmerAddress = _stock.farmer;
        moralisStok.plantName = _stock.plant.plantName;
        moralisStok.totalHarvested = _stock.totalHarvested;
        //moralisStok.harvestedDate = _stock.harvestedDate;
        //moralisStok.expirationDate = _stock.expirationDate;
        moralisStok.condition = _stock.condition;
        var result = await moralisStok.SaveAsync();
        Debug.Log("Data Tersimpan : " + result);
    }
}