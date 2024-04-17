using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
    void Start()
    {
        RuntimeCompiler runtimeCompiler = RuntimeCompiler.GetInstance();

        string script = @"
            
                public static void DynamicMethod(){
                    GameObject[] spheres = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.StartsWith(""Sphere"")).ToArray();
                    foreach (GameObject sphere in spheres) {
                        float volume = CalculateVolume(sphere.transform.localScale);
                        if (volume < 1) {
                            GameObject.Destroy(sphere);
                        }
                    }
                }

                static float CalculateVolume(Vector3 scale) {
                    return scale.x * scale.y * scale.z;
                }
        ";

        try
        {
            Action dynamicMethod = runtimeCompiler.InterpretScript(script);
            dynamicMethod.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error executing script: {e.Message}");
        }
    }
}