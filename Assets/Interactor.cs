using UnityEngine;
using UnityEngine.InputSystem;
using Whisper.Utils;

public class Interactor : MonoBehaviour
{

    public InputActionProperty toggleRecord;
    public MicrophoneRecord mr;

    private bool recording;

    // Update is called once per frame
    void Update()
    {
        int value = (int)toggleRecord.action.ReadValue<float>();
        if (!recording && value == 1)
        {
            recording = true;
            mr.StartRecord();
            Debug.Log("Started recording");
        }

        if (recording && value == 0)
        {
            recording = false;
            mr.StopRecord();
            Debug.Log("Ended recording");

        }
    }
}
