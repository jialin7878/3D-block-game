using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    public InputField username;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "9D530";
        }
    }

    public static void loginWithDeviceID()
    {
        var request = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = SystemInfo.deviceUniqueIdentifier };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
    }


    private static void OnLoginSuccess(LoginResult result)
    {
        GameManager.manager.setPlayfabID(result.PlayFabId);
        GameManager.manager.notifyForSeconds("logged in", 2.2f);
        Debug.Log("login successful");

        PlayfabData.GetAccountInfo();
    }

    private static void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login Failure: ");
        Debug.LogError(error.Error);
        GameManager.manager.notifyForSeconds("error.Error", 2.2f);
    }
}
