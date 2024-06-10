using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
    void Start()
    {
        RuntimeCompiler runtimeCompiler = RuntimeCompiler.GetInstance();

        string script = @"
            
public static void DynamicMethod(){
    // Create a forest with a specified number of trees
    int treeCount = 100;
    float areaSize = 50f;

    for (int i = 0; i < treeCount; i++)
    {
        GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tree.name = 'Tree' + i;
        tree.transform.position = new Vector3(Random.Range(-areaSize, areaSize), 0, Random.Range(-areaSize, areaSize));
        tree.transform.localScale = new Vector3(1, Random.Range(5f, 15f), 1);

        // Optionally, you can add tree leaves as spheres on top of the cylinders
        GameObject leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves.transform.parent = tree.transform;
        leaves.transform.localPosition = new Vector3(0, 1, 0);
        leaves.transform.localScale = new Vector3(3, 3, 3);
        leaves.GetComponent<Renderer>().material.color = Color.green;
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