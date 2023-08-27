using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New KUD Shop List", menuName = "KUD Shop List")]
public class KUDShopInventory : ScriptableObject
{
    //public KUDStocks KUDstock;
    public PlantObject plant;
    public string owner;
    public int amount;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string condition;
    public int price;
}
