using UnityEngine;

public class UI_Drag : MonoBehaviour
{
    bool startDrag;

    void Update()
    {
        if (startDrag)
            transform.position = Input.mousePosition;
    }

    public void StartDrag()
    {
        startDrag = true;
    }

    public void StopDrag()
    {
        startDrag = false;
    }
}
