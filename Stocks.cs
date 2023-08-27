using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stock List", menuName = "Stock List")]
[Serializable]
public class Stocks : ScriptableObject
{
    public string farmer;
    public PlantObject plant;
    public int totalHarvested;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string condition;
}
