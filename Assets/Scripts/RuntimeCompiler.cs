using System;
using Microsoft.CSharp;
using UnityEngine;
using System.Collections;
using System.CodeDom.Compiler;
using System.Reflection;

public class RuntimeCompiler : MonoBehaviour
{
    CSharpCodeProvider provider = new CSharpCodeProvider();


    public static RuntimeCompiler instance { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("There are multiple instances of the RuntimeCompiler engine in the scene.");
    }

    void OnDestroy()
    {
        instance = null;
    }

    public class MethodWrapper
    {
        System.Reflection.MethodInfo method;

        public MethodWrapper(System.Reflection.MethodInfo method)
        {
            this.method = method;
        }

        public void doIt()
        {
            IEnumerable en = (IEnumerable)method.Invoke(null, null);
        }
    }

    public System.Action InterpretScript(string script)
    {
        CompilerParameters parameters = new CompilerParameters();


        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic || assembly.Location.Contains("mscorlib.dll")) continue;
            parameters.ReferencedAssemblies.Add(assembly.Location);
        }

        parameters.GenerateExecutable = false;
        parameters.GenerateInMemory = true;
        
        CompilerResults results = provider.CompileAssemblyFromSource(parameters, GetCode(script));

        if (!results.Errors.HasErrors)
        {
            var cls = results.CompiledAssembly.GetType("DynamicCode");
            var method = cls.GetMethod("DynamicMethod", BindingFlags.Static | BindingFlags.Public);
            return (new MethodWrapper(method)).doIt;
        }
        else
        {
            foreach (CompilerError error in results.Errors)
            {
                Debug.Log(error.ErrorText);
            }

            throw new System.Exception("Dunno Man");

            //return null;
        }
    }

    public string[] GetCode(string script)
    {
        return new string[]
        {
            @"using System;
			using UnityEngine;
			using System.Collections;
			using System.Collections.Generic;

				public class DynamicCode : MonoBehaviour
				{
							
				private static DynamicCode inst;

				public static DynamicCode Instance{
					get{
						if(inst == null){
							var go = new GameObject(""DynamicCode"");
							inst = go.AddComponent<DynamicCode>();
						}
						return inst;
					}
				}


					public static void DynamicMethod(){
						DynamicCode.Instance.StartCoroutine(CR());
					}

					public static IEnumerator CR(){
						" + script + @"
						yield return new WaitForSeconds(1f);
						Debug.Log(1);
						yield return new WaitForSeconds(1f);
						Debug.Log(2);
						yield return new WaitForSeconds(1f);
						Debug.Log(3);
					}
				}
			"
        };
    }
}
