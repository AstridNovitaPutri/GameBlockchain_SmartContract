using UnityEngine;

public class RevealButton : MonoBehaviour
{
    public GameObject[] hiddenObjects;
    bool isActive = false;

    public void revealAll()
    {
        isActive = !isActive;
        foreach (GameObject obj in hiddenObjects)
            obj.SetActive(isActive);
    }
}
