using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "9D530";
        }
    }

    //public static void loginWithDeviceID()
    //{
    //    var request = new LoginWithAndroidDeviceIDRequest { CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier };
    //    PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
    //}

    public static void loginWithUsername(string username)
    {
        var request = new LoginWithPlayFabRequest { Username = username, Password = "123456"};
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess,  error => OnLoginFailure(error, username));
    }


    private static void OnLoginSuccess(LoginResult result)
    {
        GameManager.manager.setPlayfabID(result.PlayFabId);
        GameManager.manager.notifyForSeconds("logged in", 2.2f);
        Debug.Log("login successful");

        PlayfabData.GetAccountInfo();
    }

    private static void OnLoginFailure(PlayFabError error, string username)
    {
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            registerAccount(username);
            Debug.Log("New User");
        } else
        {
            Debug.LogError("Login Failure: ");
            Debug.LogError(error.Error);
            GameManager.manager.notifyForSeconds(error.ErrorMessage + "  " + error.Error, 2.2f);
        }
    }

    private static void registerAccount(string username)
    {
        var request = new RegisterPlayFabUserRequest
        {
            RequireBothUsernameAndEmail = false,
            Username = username,
            Password = "123456"
        };
        PlayFabClientAPI.RegisterPlayFabUser(request,
            result => {
                PlayfabData.UpdateDisplayName(username);
                loginWithUsername(username);
                },
            error => Debug.LogError(error.Error));
    }
}
