using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;

public class FarmerController
{

    public static async UniTask<FarmerModel> CreateUser(MoralisUser user, int Money)
    {
        FarmerModel newUser = new FarmerModel(user);
        newUser.Money = Money;
        newUser.authData.Clear();
        await newUser.SaveAsync();
        return newUser;
    }

    private class _User : FarmerModel { }

    public static async UniTask<FarmerModel> LoadFarmer(string objectId)
    {
        MoralisQuery<_User> q = await Moralis.Query<_User>();
        FarmerModel user = await q.WhereEqualTo("objectId", objectId).FirstOrDefaultAsync();
        return user;
    }

    public static async UniTask<FarmerModel> LoadFarmerByAddress(string _address)
    {
        MoralisQuery<_User> q = await Moralis.Query<_User>();
        FarmerModel user = await q.WhereEqualTo("ethAddress", _address).FirstOrDefaultAsync();
        return user;
    }

    public static async UniTask<MoralisUser> LoadFromUser(string objectId)
    {
        MoralisQuery<_User> q = await Moralis.Query<_User>();
        _User user = await q.WhereEqualTo("objectId", objectId).FirstOrDefaultAsync();
        return user;
    }

    public static async void AddMoney(int _money)
    {
        MoralisUser user = await Moralis.GetUserAsync();
        var farmer = await LoadFarmer(user.objectId);
        farmer.Money += _money;
        farmer.ACL.PublicReadAccess = true;
        farmer.ACL.PublicWriteAccess = true;
        var result = await farmer.SaveAsync();
    }

    public static async void AddMoney(FarmerModel _farmer, int _money)
    {
        _farmer.Money += _money;
        _farmer.ACL.PublicReadAccess = true;
        _farmer.ACL.PublicWriteAccess = true;
        var result = await _farmer.SaveAsync();
    }


}