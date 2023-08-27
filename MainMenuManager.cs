using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button farmButton;
    public Button shopButton;
    public Button KUDButton;
    public Button distributorButton;
    public Button konsumenButton;

    private void Start()
    {
        initializeButton();

        switch (PlayerManager.instance.playerType)
        {
            case "Farmer":
                farmButton.gameObject.SetActive(true);
                shopButton.gameObject.SetActive(true);
                KUDButton.gameObject.SetActive(false);
                distributorButton.gameObject.SetActive(false);
                konsumenButton.gameObject.SetActive(false);
                break;
            case "KUD":
                KUDButton.gameObject.SetActive(true);
                shopButton.gameObject.SetActive(true);
                farmButton.gameObject.SetActive(false);
                konsumenButton.gameObject.SetActive(false);
                distributorButton.gameObject.SetActive(false);
                break;
            case "Distributor":
                KUDButton.gameObject.SetActive(false);
                farmButton.gameObject.SetActive(false);
                shopButton.gameObject.SetActive(false);
                konsumenButton.gameObject.SetActive(false);
                distributorButton.gameObject.SetActive(true);
                break;
            case "Konsumen":
                KUDButton.gameObject.SetActive(false);
                farmButton.gameObject.SetActive(false);
                shopButton.gameObject.SetActive(false);
                konsumenButton.gameObject.SetActive(true);
                distributorButton.gameObject.SetActive(false);
                break;
            default:
                KUDButton.gameObject.SetActive(false);
                farmButton.gameObject.SetActive(false);
                shopButton.gameObject.SetActive(false);
                konsumenButton.gameObject.SetActive(false);
                distributorButton.gameObject.SetActive(false);
                break;
        }
    }
    void initializeButton()
    {
        farmButton.onClick.AddListener(() => { SingleSceneManager.instance.GoToFarm(); });
        shopButton.onClick.AddListener(() => { SingleSceneManager.instance.GoToShop(); });
        KUDButton.onClick.AddListener(() => { SingleSceneManager.instance.GoToKUD(); });
        distributorButton.onClick.AddListener(() => { SingleSceneManager.instance.GoToDistributor(); });
        konsumenButton.onClick.AddListener(() => { SingleSceneManager.instance.GoToKonsumen(); });
    }
}
