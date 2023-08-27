using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Konsumen Stock List", menuName = "Konsumen Stock List")]
public class KonsumenStock : ScriptableObject
{
    public PlantObject plant;
    public string owner;
    public string konsumen;
    public string distributor;
    public int amount;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public DateTime boughtDate;
    public int boughtPrice;
    public string condition;
    public enum Status { requested, sent, arrived, received, rejected };
    public Status status;
}
