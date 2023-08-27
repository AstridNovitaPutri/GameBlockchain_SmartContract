using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;
using UnityEngine;

public class KUDController
{
  private class _User : KUDModel { }

    public static async UniTask<KUDModel> LoadKUD(string objectId)
    {
        MoralisQuery<_User> q = await Moralis.Query<_User>();
        KUDModel kud = await q.WhereEqualTo("objectId", objectId).FirstOrDefaultAsync();
        return kud;
    }

    public static async UniTask<KUDModel> LoadKUDByAddress(string _address)
    {
        MoralisQuery<_User> q = await Moralis.Query<_User>();
        KUDModel kud         = await q.WhereEqualTo("ethAddress", _address).FirstOrDefaultAsync();
        return kud;
    }

    public static async void AddMoney(int _money)
    {
        MoralisUser user = await Moralis.GetUserAsync();

        var farmer = await LoadKUD(user.objectId);
        farmer.Money += _money;
        farmer.ACL.PublicReadAccess = true;
        farmer.ACL.PublicWriteAccess = true;

        var result = await farmer.SaveAsync();
    }

}