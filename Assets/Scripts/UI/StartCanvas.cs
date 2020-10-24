using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    public LeaderboardPanel leaderboardPanel;
    public void startGame()
    {
        GameManager.manager.startGame();
    }
}
