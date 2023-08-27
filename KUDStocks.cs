using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New KUD Stock List", menuName = "KUD Stock List")]
public class KUDStocks : ScriptableObject
{
    public string stockId;
    public PlantObject plant;
    public string owner;
    public int amount;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string boughtDate;
    public int boughtPrice;
    public string condition;
}
