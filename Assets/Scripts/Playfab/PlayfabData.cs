using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabData : MonoBehaviour
{
    public static void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request,
            infoResult => {
                GameManager.manager.setPlayfabUsername(infoResult.AccountInfo.TitleInfo.DisplayName);
            },
            error => Debug.LogError(error.Error));
    }

    //returns true if the username is already taken
    public static bool UpdateDisplayName(string newName)
    {
        bool isTaken = false;
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = newName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
        result => {
            Debug.Log("Successfully updated username");
            GameManager.manager.notifyForSeconds("updated username", 2f);
            GameManager.manager.setPlayfabUsername(newName);
        },
        error =>
        {
            Debug.LogError("Got error setting displayname");
            Debug.LogError(error.Error);
            if (error.Error == PlayFabErrorCode.NameNotAvailable)
            {
                isTaken = true;
                GameManager.manager.notifyForSeconds("username already taken", 2f);
            }
        });
        return isTaken;
    }
}
