using Photon.Pun;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UnityJSBridge : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void JS_SendChatHistory(string jsonChatHistory);

    public InputField nameInputField;
    public InputField chatInputField;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            nameInputField.text = PhotonNetwork.NickName;
            nameInputField.onValueChanged.AddListener(OnNameInputChanged);
            chatInputField.onValueChanged.AddListener(OnChatInputChanged);
        }
    }


    public void OnNameInputChanged(string newName)
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            UpdatePlayerName(newName);
        }
    }

    public void OnChatInputChanged(string message)
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            UpdateChat(message);
        }
    }

    public IEnumerator RefreshName()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdatePlayerNameTag(PhotonNetwork.NickName));
    }

    public void UpdatePlayerName(string newName)
    {
        PhotonNetwork.NickName = newName;

        // Wait for the TagObject to be set properly
        StartCoroutine(UpdatePlayerNameTag(newName));
    }

    public void UpdateChat(string message)
    {
        // Wait for the TagObject to be set properly
        StartCoroutine(UpdateChatMessage(message));

    }

    private IEnumerator UpdateChatMessage(string message)
    {
        while (PhotonNetwork.LocalPlayer.TagObject == null)
        {
            yield return null;
        }

        var localPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if (localPlayer != null)
        {
            var remoteProcedureCallService = localPlayer.GetComponentInChildren<RemoteProcedureCallService>();
            if (remoteProcedureCallService != null)
            {
                remoteProcedureCallService.SetPlayerChat(message);
                ChatHistoryService.Instance.SendMessageToServer(PhotonNetwork.NickName + ":" + message);

                Invoke("ReadHistory", 0.5f);
            }
        }
    }

    public void ReadHistory()
    {
        ChatHistoryService.Instance.GetChatHistory();
    }



    private IEnumerator UpdatePlayerNameTag(string newName)
    {
        while (PhotonNetwork.LocalPlayer.TagObject == null)
        {
            yield return null;
        }

        var localPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if (localPlayer != null)
        {
            var remoteProcedureCallService = localPlayer.GetComponentInChildren<RemoteProcedureCallService>();
            if (remoteProcedureCallService != null)
            {
                remoteProcedureCallService.SetPlayerName(newName);
            }
        }
    }

    // Method to be called from JavaScript
    public void SetPlayerNameFromJS(string newName)
    {
        UpdatePlayerName(newName);
    }

    public void SetChatFromJS(string message)
    {
        UpdateChat(message);
        ChatHistoryService.Instance.GetChatHistory();
    }

    public void SendChatHistoryToReact(string jsonChatHistory)
    {
#if !UNITY_EDITOR

        JS_SendChatHistory(jsonChatHistory);
#endif
    }
}
