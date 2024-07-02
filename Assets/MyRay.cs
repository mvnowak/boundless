using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyRay : MonoBehaviour
{
    public InputActionProperty rayAction;
    public static Vector3 point_position;
    public static GameObject point_object;
    public GameObject rightController;

    private bool casting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int value = (int)rayAction.action.ReadValue<float>();
        if (!casting && value == 1)
        {
            casting = true;
            Debug.Log("Shooting ray");
            ShootRaycast();
        }

        if (casting && value == 0)
        {
            casting = false;
        }
    }
    
    void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit))
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
            //lineRenderer.enabled = true;
            //lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // If nothing was hit, disable the line renderer
            //lineRenderer.enabled = false;
        }
    }
}
