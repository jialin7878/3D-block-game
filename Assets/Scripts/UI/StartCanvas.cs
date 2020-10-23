using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    public void startGame()
    {
        GameManager.manager.startGame();
    }
}
