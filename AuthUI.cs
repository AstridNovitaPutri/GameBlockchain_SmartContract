using UnityEngine;
using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine.SceneManagement;
using System;
using MoralisUnity;

public class AuthUI : MonoBehaviour
{
    [SerializeField] private AuthenticationKit authenticationKitObject;
    [SerializeField] private NewGameUI newGameUI;
    [SerializeField] private SelectRoleUI selectRoleUI;

    public async void Authentication_OnConnect()
    {
        authenticationKitObject.gameObject.SetActive(false);
        var user = await Moralis.GetUserAsync();
        user.ACL.PublicReadAccess = true;
        user.ACL.PublicWriteAccess = true;
        user.authData.Clear();
        await user.SaveAsync();
        newGameUI.OnCompleteSubmitUsername += OnCompleteSubmitUsername;
        selectRoleUI.OnCompleteSelectRole += OnCompleteSelectRole;
        CheckUserRole();
    }

    private void OnCompleteSubmitUsername()
    {
        selectRoleUI.gameObject.SetActive(true);
        newGameUI.gameObject.SetActive(false);
    }

    private async void OnCompleteSelectRole()
    {
        var user = await UserController.GetMyUser();
        LoadSceneByRole(user.Role);
    }

    public async void CheckUserRole()
    {
        var user = await UserController.GetMyUser();
        if(user.Role != "Farmer" || user.Role != "KUD" || user.Role != "Distributor" || user.Role != "Customer")
        {
            newGameUI.gameObject.SetActive(true);
        }

        LoadSceneByRole(user.Role);
    }

    private void LoadSceneByRole(string role)
    {
        switch (role)
        {
            case "Farmer":
                SceneManager.LoadScene("Main");
                break;
            case "KUD":
                SceneManager.LoadScene("KUD");
                break;
            case "Distributor":
                SceneManager.LoadScene("Distributor");
                break;
            case "Customer":
                SceneManager.LoadScene("Customer");
                break;
        }
    }
  
}