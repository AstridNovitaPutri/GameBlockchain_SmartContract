using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedMouse = 5f;

    private static readonly float[] BoundsX = new float[] { -500f, 50f };
    private static readonly float[] BoundsZ = new float[] { -180f, -40f };
    private static readonly float[] ZoomBounds = new float[] { 10f, 85f };

    private Camera cam;

    private Vector3 lastPanPosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMouse();
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}
