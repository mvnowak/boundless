using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Controller : MonoBehaviour
{
    public ApiHandler apiHandler;

    private string promptFilePath = "Assets/Prompts/prompt.txt";
    private string promptTemplate;

    void Start()
    {
        LoadPromptTemplate();
    }


    RuntimeCompiler runtimeCompiler = RuntimeCompiler.GetInstance();

    public async Task SpeechDetected(string speech)
    {
        if (speech.Length < 2 || speech.Contains("BLANK_AUDIO"))
        {
            Debug.Log("empty speech");
            return;
        }

        // escape double quotes in speech
        speech = speech.Replace("\"", "\\\"");
        // Insert prompt into prompt template
        string prompt = promptTemplate.Replace("<prompt>", ">" + speech + "<");

        // Get code response from GPT API
        string script = await apiHandler.GetAssistantResponse(prompt);
        //string script = "x";
        //Debug.Log("mock response");

        //Debug.Log("Received response from AI:");
        //Debug.Log(script);

        // Run code
        ExecuteScript(script);
    }

    private static string RemoveCSharpCodeBlock(string input)
    {
        // Define the regular expression pattern to match the ```csharp ... ``` block
        string pattern = @"^```(?:csharp|c#)\s*([\s\S]*?)\s*```$";
        
        // Use RegexOptions.Multiline to ensure the ^ and $ match start and end of lines
        Regex regex = new Regex(pattern, RegexOptions.Multiline);

        // Attempt to find the match
        Match match = regex.Match(input);

        if (match.Success)
        {
            // Extract the code inside the code block
            string code = match.Groups[1].Value.Trim();
            return code;
        }

        // If no match found, return the original input string
        return input;
    }

    void ExecuteScript(string script)
    {
        Debug.Log("Before removing:");
        Debug.Log(script);
        script = RemoveCSharpCodeBlock(script);
        Debug.Log("After removing:");
        Debug.Log(script);
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

    void LoadPromptTemplate()
    {
        if (!File.Exists(promptFilePath))
        {
            Debug.LogError("Prompt file not found at path: " + promptFilePath);
            return;
        }

        try
        {
            promptTemplate = File.ReadAllText(promptFilePath);
            promptTemplate = promptTemplate.Replace("\r", "").Replace("\n", "\\n").Replace("\"", "\\\"").Replace("\t", "\\t");
            
            Debug.Log("Prompt text loaded successfully");
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading the prompt file: " + e.Message);
        }
    }
}