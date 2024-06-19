using System;
using System.Collections;
using System.Collections.Generic;
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

    private string requestBody = @"
    {
        ""model"": ""gpt-3.5-turbo"",
        ""messages"": [
            {
                ""role"": ""system"",
                ""content"": ""You are a helpful assistant.""
            },
            {
                ""role"": ""user"",
                ""content"": ""Hello!""
            }
        ]
    }";

    private async void Start()
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAIApiKey}");

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                // Parse the JSON response using JsonUtility
                ChatCompletionResponse jsonResponse = JsonUtility.FromJson<ChatCompletionResponse>(responseBody);
                string assistantContent = jsonResponse.choices[0].message.content;
                Debug.Log($"Assistant's response: {assistantContent}");
            }
            else
            {
                Debug.LogError("Error: " + response.StatusCode);
                // Get the error details from the response
                var errorResponse = await response.Content.ReadAsStringAsync();
                Debug.LogError("Error Response: " + errorResponse);
            }
        }
    }
}