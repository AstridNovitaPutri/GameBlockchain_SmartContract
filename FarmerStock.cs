using MoralisUnity.Platform.Objects;
using System;

public class FarmerStock : MoralisObject
{

    public string farmerAddress;
    public string plantName;
    public int totalHarvested;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string condition;

    public FarmerStock() : base("FarmerStock") { }

}