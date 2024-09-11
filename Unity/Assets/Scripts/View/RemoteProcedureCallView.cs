using TMPro;
using UnityEngine;

public class RemoteProcedureCallView : MonoBehaviour
{
    public GameObject ChatContainer;
    public TextMeshPro playerNameText;
    public TextMeshPro playerChatText;

    public void UpdatePlayerName(string newName)
    {
        if (playerNameText != null)
        {
            playerNameText.text = newName;
        }
        else
        {
            Debug.LogError("playerNameText is not assigned.");
        }
    }

    public void UpdatePlayerChat(string message)
    {
        CancelInvoke();
        ChatHistoryService.Instance.GetChatHistory();

        if (message.Length > 0)
        {
            if (ChatContainer != null)
            {
                ChatContainer.SetActive(true);
                Invoke("Hide", 3);
            }
            else
            {
                Debug.LogError("ChatContainer is not assigned.");
            }
        }

        if (playerChatText != null)
        {
            playerChatText.text = message;
        }
        else
        {
            Debug.LogError("playerChatText is not assigned.");
        }
    }

    public void Hide()
    {
        if (ChatContainer != null)
        {
            ChatContainer.SetActive(false);
        }
        else
        {
            Debug.LogError("ChatContainer is not assigned.");
        }
    }
}
