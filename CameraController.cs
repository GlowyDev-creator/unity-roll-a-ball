using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 3.0f;
    public float zoomSpeed = 2.0f;          // How fast the camera zooms
    public float distanceFromPlayer = 10.0f;

    [Header("Vertical Constraints")]
    public float minVerticalAngle = -10.0f;
    public float maxVerticalAngle = 70.0f;

    [Header("Zoom Constraints")]
    public float minZoomDistance = 3.0f;   // How close the camera can get
    public float maxZoomDistance = 20.0f;  // How far the camera can pull away

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        if (player != null)
        {
            distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // --- CAMERA ROTATION ---
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
        }

        // --- CAMERA ZOOM ---
        // Input.GetAxis("Mouse ScrollWheel") returns positive when scrolling up, negative when scrolling down
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0.0f)
        {
            // Decrease distance to zoom in, increase to zoom out
            distanceFromPlayer -= scrollInput * zoomSpeed;
            // Clamp the zoom distance so it doesn't clip into the ball or go too far away
            distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, minZoomDistance, maxZoomDistance);
        }

        // --- POSITION & ROTATION CALCULATION ---
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);

        transform.position = player.position + (rotation * direction);
        transform.LookAt(player.position);
    }
}
