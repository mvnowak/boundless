using UnityEngine;

public class PointHandler : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public float maxVerticalAngle = 85f;

    private float verticalRotation = 0f;

    // Global variables to store hit information
    public static Vector3 point_position;
    public static GameObject point_object;

    // LineRenderer to visualize the ray
    private LineRenderer lineRenderer;

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Add LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        // Camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Raycast on 'R' key press
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShootRaycast();
        }

        // Update line renderer positions
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 100f);
        }
    }

    void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.name == "Ground")
            {
                point_position = hit.point;
                Debug.Log("Hit Ground at: " + point_position);
            }
            else
            {
                point_object = hit.collider.gameObject;
                Debug.Log("Hit object: " + point_object.name);
            }

            // Show the ray
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // If nothing was hit, disable the line renderer
            lineRenderer.enabled = false;
        }
    }
}
