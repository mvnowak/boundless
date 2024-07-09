using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class Choice
{
    public int index;
    public Message message;
    public object logprobs;
    public string finish_reason;
}

[System.Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

[System.Serializable]
public class ChatCompletionResponse
{
    public string id;
    public string @object;
    public int created;
    public string model;
    public string system_fingerprint;
    public Choice[] choices;
    public Usage usage;
}

public class ApiHandler : MonoBehaviour
{
    private string openAIApiKey = Private.ApiKey;

    // gpt-3.5-turbo or gpt-4o
    // GPT4 cost: I+O 500 * (5/1_000_000) + 500 * (15/1_000_000)
    private string requestBody = @"
    {
        ""model"": ""gpt-4o"", 
        ""messages"": [
            {
                ""role"": ""system"",
                ""content"": ""<system_prompt>""
            }
        ]
    }";
    
    public async Task<string> GetAssistantResponse(string systemPrompt)
    {
        string request = requestBody.Replace("<system_prompt>", systemPrompt);
        Debug.Log("Sending request");
        Debug.Log(request);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAIApiKey}");

        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log("Full response");
            Debug.Log(responseBody);
            // Parse the JSON response using JsonUtility
            ChatCompletionResponse jsonResponse = JsonUtility.FromJson<ChatCompletionResponse>(responseBody);
            
            string assistantContent = jsonResponse.choices[0].message.content;
            Debug.Log($"Assistant's response: {assistantContent}");
            return assistantContent;
        }

        Debug.LogError("Error: " + response.StatusCode);
        // Get the error details from the response
        var errorResponse = await response.Content.ReadAsStringAsync();
        Debug.LogError("Error Response: " + errorResponse);
        return null;
    }
}