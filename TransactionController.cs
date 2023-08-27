using System;
using System.Security.Cryptography;
using System.Text;
using MoralisUnity;
using UnityEngine;

public class TransactionController
{
    public static async void CreateFarmerKUDTransaction(FarmerShop _shopItem, int _amount, SmartContractTransferResult _smTfResult)
    {
        var transaction = Moralis.Client.Create<FarmerKUDTransaction>();
        var user = await Moralis.GetUserAsync();
    
        transaction.transactionId = DateTime.Now.ToUniversalTime().ToString().Replace("/","").Replace(":","").Replace(" ","") + DateTime.Now.Millisecond;
        transaction.stockId = _shopItem.stockId;
        transaction.boughtTime = DateTime.Now.ToUniversalTime();
        transaction.condition = _shopItem.condition;
        transaction.expirationDate = _shopItem.expirationDate;
        transaction.farmerAddress = _shopItem.farmerAddress;
        transaction.harvestedDate = _shopItem.harvestedDate;
        transaction.quantity = _amount;
        transaction.plantPrice = _shopItem.price;
        transaction.totalPrice = _amount * _shopItem.price;
        transaction.kudAddress = user.ethAddress;
        transaction.plantName = _shopItem.plantName;

        transaction.amountInMatic = _smTfResult.amountMatic;
        transaction.rateMaticInIDR = _smTfResult.rateMaticInIDR;
        transaction.transactionHash = _smTfResult.txnHash;

        await transaction.SaveAsync();
    }

    public static async void CreateKUDCustomerTransaction(KUDShop _shopItem, int _amount, SmartContractTransferResult _smTfResult)
    {
        var transaction = Moralis.Client.Create<KUDCustomerTransaction>();
        var user = await Moralis.GetUserAsync();

        transaction.transactionId = DateTime.Now.ToUniversalTime().ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + DateTime.Now.Millisecond;
        transaction.stockId = _shopItem.stockId;
        transaction.boughtTime = DateTime.Now.ToUniversalTime();
        transaction.condition = _shopItem.condition;
        transaction.expirationDate = _shopItem.expirationDate;
        transaction.farmerAddress = _shopItem.farmerAddress;
        transaction.customerAddress = _shopItem.customerAddress;
        transaction.harvestedDate = _shopItem.harvestedDate;
        transaction.quantity = _amount;
        transaction.plantPrice = _shopItem.plantPrice;
        transaction.totalPrice = _amount * _shopItem.plantPrice;
        transaction.kudAddress = user.ethAddress;
        transaction.plantName = _shopItem.plantName;

        transaction.amountInMatic = _smTfResult.amountMatic;
        transaction.rateMaticInIDR = _smTfResult.rateMaticInIDR;
        transaction.transactionHash = _smTfResult.txnHash;

        await transaction.SaveAsync();
    }


    static string GetSha256Hash(SHA256 shaHash, string input)
  {
    // Convert the input string to a byte array and compute the hash.
    byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

    // Create a new Stringbuilder to collect the bytes
    // and create a string.
    StringBuilder sBuilder = new StringBuilder();

    // Loop through each byte of the hashed data 
    // and format each one as a hexadecimal string.
    for (int i = 0; i < data.Length; i++)
    {
      sBuilder.Append(data[i].ToString("x2"));
    }

    // Return the hexadecimal string.
    return sBuilder.ToString();
  }

}