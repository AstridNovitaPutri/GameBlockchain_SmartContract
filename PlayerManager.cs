using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public string playerName;
    public string playerType;
    /*-Farmer
     *-KUD
     *-Distributor
     *-Konsumen
     */

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

    public void setPlayer(string name, string type)
    {
        playerName = name;
        playerType = type;
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public string getPlayerType()
    {
        return playerType;
    }
}
