using System;
using MoralisUnity.Platform.Objects;

public class KUDShop : MoralisObject {

    public string stockId;
    public string farmerAddress;
    public string kudAddress;
    public string customerAddress;
    public string plantName;
    public int plantPrice;
    public int quantity;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public DateTime boughtTime;
    public string condition;

    public KUDShop() : base("KUDShop") {}

}