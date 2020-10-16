using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public Spawner spawner;

    public bool isGameStarted;
    public bool isGameOver;
    public bool isGamePaused;
    public Text gameOverText;

    public Text scoreText;
    private int score = 0;

    public Canvas startMenu;
    public GameObject pauseScreen;

    void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
        } else
        {
            manager = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void incrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    void resetScore()
    {
        score = 0;
        scoreText.text = "Score: " + 0;
    }

    public void gameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;
    }

    public void restart()
    {
        gameOverText.gameObject.SetActive(false);
        isGameOver = false;
        //clear blocks
        Playfield.clearField();

        startGame();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void backToMenu()
    {
        startMenu.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        isGameStarted = false;
        isGameOver = false;
    }

    public void pauseGame()
    {
        pauseScreen.SetActive(true);
        isGamePaused = true;
    }

    public void resumeGame()
    {
        pauseScreen.SetActive(false);
        isGamePaused = false;
    }

    public void startGame()
    {
        resetScore();
        isGameStarted = true;
        spawner.spawnNext();
    }

    private void Update()
    {
        if(isGameStarted && !isGameOver)
        {
            if(!isGamePaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
            {
                pauseGame();
            }
            if(isGamePaused && Input.GetKeyDown(KeyCode.R))
            {
                resumeGame();
            }
        }

        if(isGameStarted && isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                restart();
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                backToMenu();
            }
        } else
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                pauseGame();
            }
        }
    }
}
