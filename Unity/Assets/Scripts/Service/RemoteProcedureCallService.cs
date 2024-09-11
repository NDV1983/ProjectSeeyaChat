using Photon.Pun;
using UnityEngine;

public class RemoteProcedureCallService : MonoBehaviourPun
{
    private RemoteProcedureCallView view;

    private void Start()
    {
        view = GetComponent<RemoteProcedureCallView>();
        if (view == null)
        {
            Debug.Log("RemoteProcedureCallView component not found on the GameObject.");
        }
    }

    [PunRPC]
    public void UpdateNameRPC(string newName)
    {
        if (view != null)
        {
            view.UpdatePlayerName(newName);
        }
        else
        {
            Debug.Log("View is not initialized.");
        }
    }

    [PunRPC]
    public void UpdateChatRPC(string message)
    {
        if (view != null)
        {
            view.UpdatePlayerChat(message);
        }
        else
        {
            Debug.Log("View is not initialized.");
        }
    }

    public void SetPlayerName(string newName)
    {
        photonView.RPC("UpdateNameRPC", RpcTarget.AllBuffered, newName);
    }

    public void SetPlayerChat(string message)
    {
        photonView.RPC("UpdateChatRPC", RpcTarget.AllBuffered, message);
    }
}
