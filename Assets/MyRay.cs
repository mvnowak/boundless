using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyRay : MonoBehaviour
{
    public InputActionProperty rayAction;
    public static Vector3 point_position;
    public static GameObject point_object;

    public bool recording = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int value = (int)rayAction.action.ReadValue<float>();
        if (!recording && value == 1)
        {
            recording = true;
            Debug.Log("Shooting ray");
            ShootRaycast();
        }

        if (recording && value == 0)
        {
            recording = false;
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
