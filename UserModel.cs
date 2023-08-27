using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;

public class UserModel : MoralisUser {
  
  public int Money {get;set;}

  public string Role {get;set;}
  

  public UserModel() { }

  public UserModel(MoralisUser baseUser)
  {
    base.accounts = baseUser.accounts;
    base.ACL = baseUser.ACL;
    base.authData = baseUser.authData;
    base.createdAt = baseUser.createdAt;
    base.email = baseUser.email;
    base.ethAddress = baseUser.ethAddress;
    base.objectId = baseUser.objectId;
    base.sessionToken = baseUser.sessionToken;
    base.updatedAt = baseUser.updatedAt;
    base.username = baseUser.username;
    base.ClassName = baseUser.ClassName;
  }

  

}