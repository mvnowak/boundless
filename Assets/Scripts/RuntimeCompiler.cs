using System;
using Microsoft.CSharp;
using UnityEngine;
using System.CodeDom.Compiler;
using System.Reflection;

public class RuntimeCompiler
{
    private static RuntimeCompiler _instance;
    private static readonly object LockObject = new();
    readonly CSharpCodeProvider _provider = new();

    private RuntimeCompiler()
    {
    }

    public static RuntimeCompiler GetInstance()
    {
        if (_instance != null) return _instance;
        lock (LockObject)
        {
            _instance ??= new RuntimeCompiler();
        }

        return _instance;
    }

    private class MethodWrapper
    {
        readonly MethodInfo _method;

        public MethodWrapper(MethodInfo method)
        {
            _method = method;
        }

        public void DoIt()
        {
            _method.Invoke(null, null);
        }
    }

    public Action InterpretScript(string script)
    {
        CompilerParameters parameters = new CompilerParameters();


        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic || assembly.Location.Contains("mscorlib.dll")) continue;
            parameters.ReferencedAssemblies.Add(assembly.Location);
        }

        parameters.GenerateExecutable = false;
        parameters.GenerateInMemory = true;

        CompilerResults results = _provider.CompileAssemblyFromSource(parameters, GetCode(script));

        if (!results.Errors.HasErrors)
        {
            var cls = results.CompiledAssembly.GetType("DynamicCode");
            var method = cls.GetMethod("DynamicMethod", BindingFlags.Static | BindingFlags.Public);
            return (new MethodWrapper(method)).DoIt;
        }

        foreach (CompilerError error in results.Errors)
        {
            Debug.Log(error.ErrorText);
        }

        throw new Exception("Compilation Failed");
    }

    private string[] GetCode(string script)
    {
        return new[]
        {
            @"
            using System;
            using System.Linq;
            using UnityEngine;
            using System.Collections;
            using System.Collections.Generic;
            using Random = UnityEngine.Random; // Use UnityEngine's Random

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
                " + script + @"
            }
			"
        };
    }
}