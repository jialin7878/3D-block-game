using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    public Text highscoreText;
    public Text totalLinesClearedText;

    private void OnEnable()
    {
        updateText("highscore");
        updateText("totallinescleared");
    }

    public void updateText(string name)
    {
        if(name.ToLower() == "highscore")
        {
            highscoreText.text = genText(GameManager.manager.c.highscoreLeaderboard);
        }
        if(name.ToLower() == "totallinescleared")
        {
            totalLinesClearedText.text = genText(GameManager.manager.c.totalLinesClearedLeaderboard);
        }
        
    }

    private string genText(List<PlayerLeaderboardEntry> leaderboard)
    {
        string str = "";
        foreach (PlayerLeaderboardEntry player in leaderboard)
        {
            str += player.DisplayName + ": " + player.StatValue + "\n";
        }
        return str;
    }
}
