using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabService : MonoBehaviour
{
    public string titleId = "EBE5D";

    void Start()
    {
        PlayFabSettings.TitleId = titleId;
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Playfab Login successful!");
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }
}
