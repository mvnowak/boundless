using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float sensitivity = 5.0f;
    public float rayLength = 100.0f;
    public LineRenderer lineRenderer;

    public static Vector3 point_position = Vector3.zero;
    public static GameObject point_object = null;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Setup the line renderer
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Rotate camera with mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        mainCamera.transform.Rotate(-mouseY, mouseX, 0);
        mainCamera.transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, 0);

        // Shoot raycast on pressing "R"
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(mainCamera.transform.position + new Vector3(0, -0.1f, 0), mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Update the line renderer to show the ray
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.gameObject.name == "Ground")
            {
                point_position = hit.point;
                Debug.Log("Hit ground plane");
            }
            else
            {
                Debug.Log("Hit object" + hit.collider.gameObject.name);
                point_object = hit.collider.gameObject;
            }
        }
        else
        {
            // If the ray doesn't hit anything, extend the line to the max ray length
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);
            point_position = Vector3.zero;
            point_object = null;
        }
    }
}
