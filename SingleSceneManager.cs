using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleSceneManager : MonoBehaviour
{
    public static SingleSceneManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void SwitchScene(string to, string from)
    {
        SceneManager.LoadSceneAsync(to);
        SceneManager.UnloadSceneAsync(from);
    }

    public void GoToFarm()
    {
        SwitchScene("Main", SceneManager.GetActiveScene().name);
    }

    public void GoToShop()
    {
        SwitchScene("Buyer", SceneManager.GetActiveScene().name);
    }

    public void GoToDistributor()
    {
        SwitchScene("Distributor", SceneManager.GetActiveScene().name);
    }

    public void GoToKonsumen()
    {
        SwitchScene("Konsumen", SceneManager.GetActiveScene().name);
    }

    public void GoToKUD()
    {
        SwitchScene("KUD", SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SwitchScene("MainMenu", SceneManager.GetActiveScene().name);
    }
}
