using System;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserController
{
    public enum UserRole
    {
        Farmer      = 0,
        KUD         = 1,
        Distributor = 2,
        Customer    = 3
    }

  public static async UniTask<UserModel> CreateUser(MoralisUser user){
    UserModel newUser = new UserModel(user);
    await newUser.SaveAsync();
    return newUser;
  }

  internal class _User : UserModel { }

  public static async UniTask<UserModel> GetUser(string objectId){
    MoralisQuery<_User> q = await Moralis.Query<_User>();
    _User user = await q.WhereEqualTo("objectId", objectId).FirstOrDefaultAsync();
    return user;
  }

  public static async UniTask<UserModel> GetUserFromETHAddress(string _ethAddress){
    MoralisQuery<_User> q = await Moralis.Query<_User>();
    _User user = await q.WhereEqualTo("ethAddress", _ethAddress).FirstOrDefaultAsync();
    return user;
  }

  public static async UniTask<UserModel> GetMyUserFromETHAddress(){
    var user = await Moralis.GetUserAsync();
    return await GetUserFromETHAddress(user.ethAddress);
  }

  public static async UniTask<UserModel> GetMyUser(){
    var user = await Moralis.GetUserAsync();
    return await GetUser(user.objectId);
  }

  public static async UniTask<UserModel> LoadFromUser(UserModel user){
    return await GetUser(user.objectId);
  }

  public static async void AddMoney(int _amount){
    var user = await GetMyUser();
    user.Money += _amount;
    user.ACL.PublicReadAccess = true;
    user.ACL.PublicWriteAccess = true;
    user.authData.Clear();
    await user.SaveAsync();
  }

  public static async void AddMoney(UserModel _user, int _amount){
    _user.Money += _amount;
    _user.ACL.PublicReadAccess = true;
    _user.ACL.PublicWriteAccess = true;
    _user.authData.Clear();
    await _user.SaveAsync();
  }

  public static async void SendETH(string _address){
    MoralisUser user = await Moralis.GetUserAsync();
    float transferAmount = 0.001f;
    string fromAddress = user.authData["moralisEth"]["id"].ToString();
    string toAddress = _address;
    TransactionInput txnRequest = new TransactionInput(){
      Data = String.Empty,
      From = fromAddress,
      To = toAddress,
      Value = new HexBigInteger(UnitConversion.Convert.ToWei(transferAmount))
    };
    try{
      string txnHash = await Moralis.Web3Client.Eth.TransactionManager.SendTransactionAsync(txnRequest);
      Debug.Log($"Transfered {transferAmount} ETH from {fromAddress} to {toAddress}.  TxnHash: {txnHash}");
    }
    catch (Exception exp){
      Debug.Log($"Transfer of {transferAmount} ETH from {fromAddress} to {toAddress} failed! with error {exp}");
    }
  }

}