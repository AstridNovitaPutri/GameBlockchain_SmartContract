using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectRoleUI : MonoBehaviour {
  
  public Button farmerButton;
  public Button KUDButton;
    public Button distributorButton;
    public Button customerButton;
    public Action OnCompleteSelectRole;

  private void Start() {  
    farmerButton.onClick.AddListener(OnClickFarmerButton);
    KUDButton.onClick.AddListener(OnClickKUDButton);
        distributorButton.onClick.AddListener(OnClickDistributorButton);
        customerButton.onClick.AddListener(OnClickCustomerButton);
    }

  private async void OnClickFarmerButton(){
    var user = await UserController.GetMyUser();
    user.Role = "Farmer";
    user.Money = 5000;
    user.authData.Clear();
    var result = await user.SaveAsync();
    SceneManager.LoadScene("Main");
    Debug.Log("KUD Selected");
  }

  private async void OnClickKUDButton(){
    var user = await UserController.GetMyUser();
    user.Role = "KUD";
    user.Money = 1000000;
    user.authData.Clear();
    var result = await user.SaveAsync();
    SceneManager.LoadScene("KUD");
    Debug.Log("KUD Selected");
  }

    private async void OnClickDistributorButton()
    {
        var user = await UserController.GetMyUser();
        user.Role = "Distributor";
        user.Money = 1000000;
        user.authData.Clear();
        var result = await user.SaveAsync();
        SceneManager.LoadScene("Distributor");
        Debug.Log("Distributor Selected");
    }

    private async void OnClickCustomerButton()
    {
        var user = await UserController.GetMyUser();
        user.Role = "Customer";
        user.Money = 1000000;
        user.authData.Clear();
        var result = await user.SaveAsync();
        SceneManager.LoadScene("Customer");
        Debug.Log("Customer Selected");
    }
}