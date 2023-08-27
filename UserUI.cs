using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Button settingsButton;

    [SerializeField] Button refreshButton;

    [SerializeField] Button addMoneyButton;

    [SerializeField] Button gotoShopButton;

    [SerializeField] Button backToGameButton;

    [SerializeField] Button historyButton;
    [SerializeField] GameObject history;

    public Action OnClickSettingButton;

    private async void OnEnable()
    {
        Moralis.Start();
        var user = await Moralis.GetUserAsync();
        usernameText.text = user.username;
        var userCustom = await UserController.GetUser(user.objectId);
        moneyText.text = "Rp." + userCustom.Money.ToString();

        AssignButtons();

        if (SceneManager.GetActiveScene().name == "Buyer")
        {
            if (backToGameButton != null) backToGameButton.gameObject.SetActive(true);
            if (gotoShopButton != null) gotoShopButton.gameObject.SetActive(false);
            if (userCustom.Role == "Farmer")
            {
                if (backToGameButton != null) backToGameButton.GetComponentInChildren<Text>().text = "Back to Farm";
            }
            else if (userCustom.Role == "KUD")
            {
                if (backToGameButton != null) backToGameButton.GetComponentInChildren<Text>().text = "Back to KUD";
            }
            else if(userCustom.Role == "Customer")
            {
                if (backToGameButton != null) backToGameButton.GetComponentInChildren<Text>().text = "Back to Customer";
            }
        }
        else
        {
            if (backToGameButton != null) backToGameButton.gameObject.SetActive(false);
            if (gotoShopButton != null) gotoShopButton.gameObject.SetActive(true);
        }

        TestDateData();
    }

    private void AssignButtons()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(() => OnClickSettingButton.Invoke());
        }

        if (addMoneyButton != null)
        {
            addMoneyButton.onClick.RemoveAllListeners();
            addMoneyButton.onClick.AddListener(AddMoney);
        }

        if (refreshButton != null)
        {
            refreshButton.onClick.RemoveAllListeners();
            refreshButton.onClick.AddListener(GetStock);
        }

        if (gotoShopButton != null)
        {
            gotoShopButton.onClick.RemoveAllListeners();
            gotoShopButton.onClick.AddListener(GotoShop);
        }

        if (backToGameButton != null)
        {
            backToGameButton.onClick.RemoveAllListeners();
            backToGameButton.onClick.AddListener(BackToGame);
        }

        if (historyButton != null)
        {
            historyButton.onClick.RemoveAllListeners();
            historyButton.onClick.AddListener(ActiveHistory);
        }
    }

    public async void updateUI()
    {
        var user = await Moralis.GetUserAsync();
        var userCustom = await UserController.GetUser(user.objectId);
        moneyText.text = "Rp." + userCustom.Money.ToString();
    }

    private async void BackToGame()
    {
        var user = await Moralis.GetUserAsync();
        var userCustom = await UserController.GetUser(user.objectId);
        if (userCustom.Role == "Farmer")
        {
            SceneManager.LoadScene("Main");
        }
        else if (userCustom.Role == "KUD")
        {
            SceneManager.LoadScene("KUD");
        }
        else if(userCustom.Role == "Customer")
        {
            SceneManager.LoadScene("Customer");
        }
    }

    private void GetStock()
    {
        FindObjectOfType<UserUI>().updateUI();
    }

    private async void AddMoney()
    {
        MoralisUser user = await Moralis.GetUserAsync();
        var farmer = await FarmerController.LoadFarmer(user.objectId);
        farmer.Money += 5000;
        var result = await farmer.SaveAsync();
    }

    private void GotoShop()
    {
        SceneManager.LoadScene("Buyer");
    }

    private void ActiveHistory()
    {
        history.gameObject.SetActive(true);
    }

    private async void TestDateData()
    {
        MoralisQuery<FarmerStock> stockQ = await Moralis.Query<FarmerStock>();
        IEnumerable<FarmerStock> result = await stockQ.FindAsync();
        var user = await UserController.GetMyUser();
        DateTime from = new DateTime(2022, 9, 20), to = new DateTime(2022, 9, 22);
        foreach (FarmerStock stock in result)
        {
            if (stock.farmerAddress == user.ethAddress)
            {
                if (stock.harvestedDate >= from && stock.harvestedDate <= to)
                    Debug.Log(stock.plantName);
            }
            else
            {
                //Debug.Log("False");
            }
        }
    }

}