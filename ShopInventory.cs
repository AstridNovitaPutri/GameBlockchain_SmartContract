using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Inventory", menuName = "Shop Inventory")]
public class ShopInventory : ScriptableObject
{
    //public Stocks stock;
    public string stockId;
    public PlantObject plant;
    public string owner;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string condition;
    public int quantity;
    public int price;
}
