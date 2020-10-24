using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public Spawner spawner;
    public Constants c;

    public bool isGameStarted;
    public bool isGameOver;
    public bool isGamePaused;
    private int score = 0;

    public bool isLoading;

    #region Events
    public event Action<int> OnPlayerScore;
    public event Action OnPlayerPause;
    public event Action OnPlayerResume;
    public event Action OnGameOver;
    public event Action OnHoldBlock;
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

    public void login()
    {
        if(usernameField.text.Length <= 4 || usernameField.text.Length > 24)
        {
            notifyForSeconds("username must be between 4 nd 25 characters", 2f);
        } else
        {
            PlayfabLogin.loginWithUsername(usernameField.text);
            StartCoroutine(load(() => backToMenu()));
        }
    }

    public IEnumerator load(Action onLoaded)
    {
        yield return new WaitWhile(() => isLoading);
        onLoaded?.Invoke();
    }

    public void setPlayfabID(String id)
    {
        c.PlayfabID = id;
    }

    public void setPlayfabUsername(String name)
    {
        c.displayName = name;
        username.text = name;
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
        Debug.Log("game over");
        isGameOver = true;
        OnGameOver?.Invoke();
        PlayfabData.updatePlayerStats(score);
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

        SceneManager.LoadScene((int) SceneIndex.start);
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
        AsyncOperation op = SceneManager.LoadSceneAsync((int) SceneIndex.game);
        while (!op.isDone)
        {
            yield return null;
        }
        spawnNext();
    }

    public void spawnNext()
    {
        spawner.spawnNext();
        OnBlockSpawned?.Invoke(spawner.getNext());
    }

    void holdBlock()
    {
        //int i = spawner.holdBlock();
        //OnHoldBlock?.Invoke();
        //if(i == -1)
        //{
        //    spawnNext();
        //}
    }

    private void Update()
    {

        if (isGameStarted && !isGameOver)
        {
            if(!isGamePaused)
            {
                if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseGame();
                }
                if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    holdBlock();
                }
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
        } else
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                pauseGame();
            }
        }
    }

    #region UI

    public GameObject notifNoButton;
    public Text username;

    public InputField usernameField;

    public void notifyForSeconds(string message, float seconds)
    {
        StartCoroutine(wait(message, seconds));
    }

    IEnumerator wait(string message, float seconds)
    {
        notifNoButton.SetActive(true);
        notifNoButton.GetComponentInChildren<Text>().text = message;
        yield return new WaitForSeconds(seconds);
        notifNoButton.SetActive(false);
    }

    #endregion
}

enum SceneIndex
{
    load = 0,
    start = 1,
    game = 2
}
