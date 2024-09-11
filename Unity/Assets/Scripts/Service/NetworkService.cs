using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkService : MonoBehaviourPunCallbacks
{
    private UnityJSBridge unityJSBridge;

    void Start()
    {
        unityJSBridge = GetComponent<UnityJSBridge>();
        // Handle keyboard input for WebGL
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLInput.captureAllKeyboardInput = false;
#endif
        // Connect to Photon servers
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.TransportProtocol = ExitGames.Client.Photon.ConnectionProtocol.WebSocketSecure;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master. Starting coroutine to join lobby...");
        // Start a coroutine to join the lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Creating or Joining Room...");
        // Now that we're in the lobby, join or create a room
        PhotonNetwork.JoinOrCreateRoom("DefaultRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer.IsLocal == false)
        {
            StartCoroutine(unityJSBridge.RefreshName());
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message}");
    }

    public override void OnJoinedRoom()
    {
        // Spawn the player once joined to the room
        Debug.Log("Joined Room. Spawning Player...");
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Ensure this is only called once per player
        if (PhotonNetwork.LocalPlayer.TagObject == null)
        {
            Debug.Log("Instantiating player...");
            var player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.TagObject = player;
        }
        else
        {
            Debug.Log("Player already instantiated.");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from Photon with reason {cause}");
    }

    void OnApplicationQuit()
    {
        // Mark this instance as not running when the application quits
        PlayerPrefs.SetInt("GameInstance", 0);
        PlayerPrefs.Save();
    }
}
