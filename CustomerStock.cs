using MoralisUnity.Platform.Objects;
using System;

public class CustomerStock : MoralisObject
{
    public string stockId;
    public string farmerAddress;
    public string kudAddress;
    public string customerAddress;
    public string distributorAddress;
    public string plantName;
    public int plantPrice;
    public int quantity;
    public DateTime harvestedDate;
    public DateTime expirationDate;
    public DateTime sendDate = DateTime.MinValue;
    public DateTime receivedDate = DateTime.MinValue;
    public DateTime boughtTime;
    public string condition;
    public int status;

    public string GetStatus()
    {
        switch (status)
        {
            case 0: return "dipesan";
            case 1: return "sedang dikirim";
            case 2:
                if (DateTime.Now.ToUniversalTime() > receivedDate)
                    return "diterima";
                else
                    return "dalam perjalanan";
            default: return "dipesan";
        }
    }

    public bool IsCompleted()
    {
        return status > 0;
    }

    public CustomerStock() : base("CustomerStock") { }
}
