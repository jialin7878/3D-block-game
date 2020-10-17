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
    public event Action<int> OnBlockSpawned;
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
        isGameStarted = true;

        resetScore();
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        while (!op.isDone)
        {
            yield return null;
        }
        spawnNext();
    }

    public void spawnNext()
    {
        Debug.Log("called spawn next");
        spawner.spawnNext();
        OnBlockSpawned?.Invoke(spawner.getNext());
    }

    private void Update()
    {
# if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.U))
        {
            spawnNext();
        }
# endif

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
