using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoralisUnity;
using System;

public class NewGameUI : MonoBehaviour {
  public TMP_InputField userNameInput;
  public TMP_InputField emailInput;

  public Button saveButton;

  public Action OnCompleteSubmitUsername;

  private async void OnEnable() {
    var user = await Moralis.GetUserAsync();
    userNameInput.text = user.username;
    emailInput.text = user.email;
    saveButton.onClick.AddListener(SubmitUsernameAndEmail);
  }

  private async void SubmitUsernameAndEmail(){
    var user = await Moralis.GetUserAsync();
    user.username = userNameInput.text;
    user.email = emailInput.text;
    user.ACL.PublicReadAccess = true;
    user.ACL.PublicWriteAccess = true;
    user.authData.Clear();
    var result = await user.SaveAsync();
    OnCompleteSubmitUsername.Invoke();
  }


}