using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public Spawner spawner;

    public bool isGameStarted;
    public bool isGameOver;
    public bool isGamePaused;
    private int score = 0;

    #region Events
    public event Action<int> OnPlayerScore;
    public event Action OnPlayerPause;
    public event Action OnPlayerResume;
    public event Action OnGameOver;



    #endregion


    void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
        } else
        {
            manager = this;
        }
        DontDestroyOnLoad(this);
    }

    public void incrementScore()
    {
        score++;
        OnPlayerScore?.Invoke(score);
    }

    void resetScore()
    {
        score = 0;
        OnPlayerScore?.Invoke(score);
    }

    public void gameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
    }

    public void restart()
    {
        isGameOver = false;
        //reload scene, destroying gameobjects
        SceneManager.LoadScene(1);
        //reset matrix
        Playfield.clearField();

        startGame();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void backToMenu()
    {
        isGameStarted = false;
        isGameOver = false;

        SceneManager.LoadScene(0);
    }

    public void pauseGame()
    {
        isGamePaused = true;
        OnPlayerPause?.Invoke();
    }

    public void resumeGame()
    {
        isGamePaused = false;
        OnPlayerResume?.Invoke();
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
        resetScore();
        isGameStarted = true;
        spawner.spawnNext();
    }


#if (UNITY_EDITOR)
    //for myself
    void startFromGameScreen()
    {
        isGameStarted = true;
        spawner.spawnNext();
    }
#endif

    private void Update()
    {
#if (UNITY_EDITOR)
        if(!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                startFromGameScreen();
            }
        }
#endif

        if (isGameStarted && !isGameOver)
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
