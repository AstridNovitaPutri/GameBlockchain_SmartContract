using System;
using MoralisUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour {

  public TMP_InputField userNameInput;
  public Button saveButton;

  public Action OnClickSaveButton;
  
  private async void OnEnable() {
    var user = await Moralis.GetUserAsync();
    userNameInput.text = user.username;
    saveButton.onClick.AddListener(SubmitUsername);
  }

  private async void SubmitUsername(){
    var user = await Moralis.GetUserAsync();
    user.username = userNameInput.text;
    user.authData.Clear();
    var result = await user.SaveAsync();
    OnClickSaveButton.Invoke();
  }

}