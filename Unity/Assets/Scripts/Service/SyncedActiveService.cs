using Photon.Pun;
using UnityEngine;

public class SyncedActiveService : MonoBehaviourPun, IPunObservable
{
    public GameObject SyncedActiveGameObject;
    private bool isSynced;

    void Start()
    {
        // Ensure the lamp's initial state is consistent across the network
        isSynced = SyncedActiveGameObject.activeSelf;
    }

    void OnMouseDown()
    {
        photonView.RPC("ChangeState", RpcTarget.All);
    }

    [PunRPC]
    void ChangeState()
    {
        isSynced = !isSynced;
        SyncedActiveGameObject.SetActive(isSynced);
    }

    // Synchronize the lamp state across the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the current state of the lamp to other players
            stream.SendNext(isSynced);
        }
        else
        {
            // Receive the current state of the lamp from the network
            isSynced = (bool)stream.ReceiveNext();
            SyncedActiveGameObject.SetActive(isSynced);
        }
    }
}
