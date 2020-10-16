using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public Spawner spawner;

    private bool isGameOver;
    public Text gameOverText;

    public Text scoreText;
    private int score = 0;

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

        //start first game
        spawner.spawnNext();
    }

    public void incrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
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
        //spawn first block
        spawner.spawnNext();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void pauseGame()
    {
        
    }

    private void Update()
    {
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                restart();
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                quit();
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
