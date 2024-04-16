using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
    /**
     * User
       write the BODY of a coroutine for unity, that spawns a cube and then lets it bob up and down with a sine wave.
       don't give me the surrounding class, don't add using directives, your code will be DYNAMICALLY inserted into the
       body of a coroutine. everything has to happen in this method body. so i only need the CONTENT, nothing around it.
     */
    void Start()
    {
        // Create an instance of the RuntimeCompiler class
        RuntimeCompiler runtimeCompiler = RuntimeCompiler.GetInstance();

        // Define the C# script to be executed dynamically
        string script = @"
                    Vector3 spawnPosition = new Vector3(0f, 0.5f, 0f);
                    Quaternion spawnRotation = Quaternion.identity;
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = spawnPosition;
                    cube.transform.rotation = spawnRotation;
        
                    float bobSpeed = 2f;
                    float bobMagnitude = 0.5f;
                    float time = 0f;
        
                    while (true)
                    {
                        time += Time.deltaTime;
                        float yOffset = Mathf.Sin(time * bobSpeed) * bobMagnitude;
                        cube.transform.position = spawnPosition + new Vector3(0f, yOffset, 0f);
                        yield return null;
                    }
                ";

        try
        {
            // Compile and execute the script
            Action dynamicMethod = runtimeCompiler.InterpretScript(script);

            // Invoke the dynamically compiled method
            dynamicMethod.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error executing script: {e.Message}");
        }
    }
}