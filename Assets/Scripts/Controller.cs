using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
    void Start()
    {
        RuntimeCompiler runtimeCompiler = RuntimeCompiler.GetInstance();

        string script = @"
            
public static void DynamicMethod(){
    GameObject[] spheres = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains('Sphere')).ToArray();
    foreach (GameObject sphere in spheres)
    {
        GameObject.Destroy(sphere);
    }

    GameObject[] cubes = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains('Cube')).ToArray();
    GameObject biggestCube = cubes.OrderByDescending(cube => cube.transform.localScale.magnitude).FirstOrDefault();
    if (biggestCube != null)
    {
        Renderer cubeRenderer = biggestCube.GetComponent<Renderer>();
        cubeRenderer.material.color = Color.green;
        DynamicCode.Instance.StartCoroutine(SpinCube(biggestCube.transform));
    }
}

public static IEnumerator SpinCube(Transform cubeTransform)
{
    while (true)
    {
        cubeTransform.Rotate(Vector3.up, 50f * Time.deltaTime);
        yield return null;
    }
}
        ";
        // replace single quotes with double quotes
        script = script.Replace("'", "\"");
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