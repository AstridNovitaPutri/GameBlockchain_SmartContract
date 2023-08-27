//using directives
using MoralisUnity;
using Nethereum.Util;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

public class SmartContractCall : MonoBehaviour
{
    public double rateMaticInIDR;
    public bool useBinanceAPI;

    [ContextMenu("GetMaticInBidrPrice")]
    public async Task<string> GetMaticInBidrPrice()
    {
        string url = "https://api.binance.com/api/v3/ticker/price";
        string result = "";

        try
        {
            HttpWebRequest myRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
            using (WebResponse myResponse = await myRequest.GetResponseAsync())
            {
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }

            var resultObj = new[]
            {
                new
                {
                    symbol = string.Empty,
                    price = string.Empty
                }
            };

            var results = JsonConvert.DeserializeAnonymousType(result, resultObj);
            foreach(var item in results)
            {
                Debug.Log(item.symbol);
                if (item.symbol == "MATICBIDR") return item.price;
            }

            return string.Empty;

        } catch (Exception e){
            Debug.LogError(e);
        }

        return string.Empty;
    }

    public async Task<SmartContractTransferResult> SendRawMatic(string fromAddress, string toAddress, double amountInIDR)
    {
        double priceInMatic;
        if (useBinanceAPI)
        {
            var price = await GetMaticInBidrPrice();
            if (!string.IsNullOrEmpty(price))
            {
                rateMaticInIDR = Convert.ToDouble(price);
                priceInMatic = amountInIDR / rateMaticInIDR;
            }
            else
                priceInMatic = amountInIDR / rateMaticInIDR;
        }
        else
        {
            priceInMatic = amountInIDR / rateMaticInIDR;
        }

        Debug.Log("priceInMatic: " + priceInMatic);

        float transferAmount = (float)priceInMatic;

        // Create transaction request.
        TransactionInput txnRequest = new TransactionInput()
        {
            Data = String.Empty,
            //current connected user
            From = fromAddress,
            //reciever address
            To = toAddress,
            // amount in  wei
            Value = new HexBigInteger(UnitConversion.Convert.ToWei(transferAmount)) // convert to WEI
        };
        try
        {
            // Execute the transaction.
            string txnHash = await Moralis.Web3Client.Eth.TransactionManager.SendTransactionAsync(txnRequest);

            Debug.Log($"Transfered {transferAmount} ETH from {fromAddress} to {toAddress}.  TxnHash: {txnHash}");

            return new SmartContractTransferResult(true, txnHash, transferAmount, (float)rateMaticInIDR, "transfer success");
        }
        catch (Exception exp)
        {
            Debug.Log($"Transfer of {transferAmount} ETH from {fromAddress} to {toAddress} failed! with error {exp}");
            return new SmartContractTransferResult(false, string.Empty, transferAmount, (float)rateMaticInIDR, exp.Message);
        }
    }
}

public class SmartContractTransferResult
{
    public bool isSuccess;
    public string txnHash;
    public float amountMatic;
    public float rateMaticInIDR;
    public string message;

    public SmartContractTransferResult(bool isSuccess, string txnHash, float amount, float rateMaticInIDR, string message)
    {
        this.isSuccess = isSuccess;
        this.txnHash = txnHash;
        this.amountMatic = amount;
        this.rateMaticInIDR = rateMaticInIDR;
        this.message = message;
    }
}