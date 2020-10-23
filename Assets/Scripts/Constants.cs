using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Constants")]
public class Constants : ScriptableObject
{
    public string PlayfabID;
    public string displayName;

    public List<PlayerLeaderboardEntry> highscoreLeaderboard;
    public List<PlayerLeaderboardEntry> totalLinesClearedLeaderboard;

    public static Color inactiveColor;
    public static Color activeColor;
}
