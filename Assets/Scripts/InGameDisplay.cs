using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameDisplay : MonoBehaviour
{
    public Sprite[] displays;

    public GameObject pauseScreen;
    public Text scoreText;
    public Text gameOverText;
    public Image blockDisplay;

    private void Start()
    {
        GameManager.manager.OnPlayerPause += showPauseScreen;
        GameManager.manager.OnPlayerResume += hidePauseScreen;
        GameManager.manager.OnPlayerScore += setScoreDisplay;
        GameManager.manager.OnGameOver += showGameOver;
        GameManager.manager.OnBlockSpawned += setBlockDisplay;
    }
    public void showGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    public void setScoreDisplay(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void setBlockDisplay(int index)
    {
        blockDisplay.sprite = displays[index];
    }

    public void showPauseScreen()
    {
        pauseScreen.SetActive(true);
    }

    public void hidePauseScreen()
    {
        pauseScreen.SetActive(false);
    }
}
