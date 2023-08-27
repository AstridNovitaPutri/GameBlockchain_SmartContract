using MoralisUnity;
using MoralisUnity.Kits.AuthenticationKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Web3UIParent : MonoBehaviour
{

  [SerializeField] private GameObject authenticationKitObject;

  [SerializeField] private SelectRoleUI selectRoleUI;

  [SerializeField] private UserUI userUIObject;
  

  [SerializeField] private SettingUI settingsPanelObject;

  [SerializeField] private SmartContract smartContract;

  private AuthenticationKit authKit;

  private void Awake() {
    InitUIState();
  }

  private void Start()
  {
    Moralis.Start();
    authKit = authenticationKitObject.GetComponent<AuthenticationKit>();
    userUIObject.OnClickSettingButton += ShowSettingPanelUI;
    settingsPanelObject.OnClickSaveButton += HideSettingPanelUI;
    smartContract.gameObject.SetActive(false);
  }

  private void InitUIState(){
    authenticationKitObject.SetActive(true);
    userUIObject.gameObject.SetActive(false);
    settingsPanelObject.gameObject.SetActive(false);
  }

  public void Authentication_OnConnect()
  {
    authenticationKitObject.SetActive(false);
    userUIObject.gameObject.SetActive(true);
    //CheckUserRole();
    //smartContract.gameObject.SetActive(true);
  }

  public async void CheckUserRole(){
    var user = await UserController.GetMyUser();
    if(user.Role != "Farmer" || user.Role != "KUD"){
      selectRoleUI.gameObject.SetActive(true);
    }
    if(user.Role == "Farmer"){
      SceneManager.LoadScene("Main");
    }else if(user.Role == "KUD"){
      SceneManager.LoadScene("KUD");
    }
  }

  public void LogoutButton_OnClicked()
  {
    authKit.Disconnect();
    authenticationKitObject.SetActive(true);
    userUIObject.gameObject.SetActive(false);
    //smartContract.gameObject.SetActive(false);
  }

  private void ShowSettingPanelUI()
  {
    settingsPanelObject.gameObject.SetActive(true);
    userUIObject.gameObject.SetActive(false);
    //smartContract.gameObject.SetActive(false);
  }

  private void HideSettingPanelUI()
  {
    settingsPanelObject.gameObject.SetActive(false);
    userUIObject.gameObject.SetActive(true);
    //smartContract.gameObject.SetActive(true);
  }
}
