using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    public InputActionProperty test;

    public bool recording = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int value = (int)test.action.ReadValue<float>();
        if (!recording && value == 1)
        {
            recording = true;
            Debug.Log("Started recording");
        }

        if (recording && value == 0)
        {
            recording = false;
            Debug.Log("Ended recording");

        }
    }
}
