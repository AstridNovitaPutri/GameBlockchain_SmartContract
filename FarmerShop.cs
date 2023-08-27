using System;
using MoralisUnity.Platform.Objects;

public class FarmerShop : MoralisObject {
    
    public string stockId;
    public string farmerAddress;
    public string plantName;
    public int price;
    public int quantity;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public string condition;

    public FarmerShop() : base("FarmerShop") {}

}