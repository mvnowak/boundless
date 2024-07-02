using UnityEngine;
using UnityEngine.InputSystem;
using Whisper.Samples;
using Whisper.Utils;

public class Interactor : MonoBehaviour
{

    public InputActionProperty toggleRecord;
    public MicrophoneDemo md;

    private bool recording;

    // Update is called once per frame
    void Update()
    {
        int value = (int)toggleRecord.action.ReadValue<float>();
        if (!recording && value == 1)
        {
            recording = true;
            md.OnButtonPressed();
            Debug.Log("Started recording");
        }

        if (recording && value == 0)
        {
            recording = false;
            md.OnButtonPressed();
            Debug.Log("Ended recording");

        }
    }
}
