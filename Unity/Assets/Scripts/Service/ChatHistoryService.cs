using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;

public class ChatHistoryService : MonoBehaviour
{
    // Singleton instance
    public static ChatHistoryService Instance { get; private set; }

    private UnityJSBridge unityJSBridge;



    private void Awake()
    {
        unityJSBridge = GetComponent<UnityJSBridge>();
        // Ensure only one instance of the service exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SendMessageToServer(string messageText)
    {
        StartCoroutine(SendMessageCoroutine(messageText));
    }

    private IEnumerator SendMessageCoroutine(string messageText)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "SendMessage",
            FunctionParameter = new { message = messageText },
            GeneratePlayStreamEvent = true
        };

        bool isRequestDone = false;

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            if (result.Error != null)
            {
                Debug.LogError("Error sending message: " + result.Error.Message);
            }
            else
            {
                Debug.Log("Message sent successfully!");
            }
            isRequestDone = true;
        },
        error =>
        {
            Debug.LogError("Error sending message: " + error.GenerateErrorReport());
            isRequestDone = true;
        });

        yield return new WaitUntil(() => isRequestDone);
    }


    public void GetChatHistory()
    {
        StartCoroutine(GetChatHistoryCoroutine());
    }

    private IEnumerator GetChatHistoryCoroutine()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "GetChatHistory",
            GeneratePlayStreamEvent = true
        };

        bool isRequestDone = false;
        string chatHistoryJson = "";

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            if (result.Error != null)
            {
                Debug.LogError("Error getting chat history: " + result.Error);
            }
            else
            {
                chatHistoryJson = result.FunctionResult.ToString();
                unityJSBridge.SendChatHistoryToReact(chatHistoryJson);
            }
            isRequestDone = true;
        },
        error =>
        {
            Debug.LogError("Error getting chat history: " + error.GenerateErrorReport());
            isRequestDone = true;
        });

        yield return new WaitUntil(() => isRequestDone);
    }


}
