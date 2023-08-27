using System;
using MoralisUnity.Platform.Objects;

public class FarmerKUDTransaction : MoralisObject {

    public string transactionId;
    public string stockId;
    public string farmerAddress;
    public string kudAddress;
    public string plantName;
    public int plantPrice;
    public int quantity;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public DateTime boughtTime;
    public string condition;
    public int totalPrice;

    public float amountInMatic;
    public float rateMaticInIDR;
    public string transactionHash;

    public FarmerKUDTransaction() : base("FarmerKUDTransaction") {}

}