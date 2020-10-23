using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameInput : MonoBehaviour
{
    public Text placeholder;
    public Text userInput;

    private void OnEnable()
    {
        string initialUsername = GameManager.manager.c.displayName;
        placeholder.text = initialUsername;
    }

    public void updateUsername()
    {
        string newName = userInput.text;
        if(newName.Length < 4 || newName.Length > 24)
        {
            GameManager.manager.notifyForSeconds("username must be between 4 and 25 characters", 3f);
        } else
        {
            PlayfabData.UpdateDisplayName(newName);
        }
    }
}
