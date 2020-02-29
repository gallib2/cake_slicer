using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static event Action OnGameOver;
    public static event Action<int> OnWin;
    public static event Action OnLose;
    public static event Action OnLevelInitialised;

    public static string playerName;
    public static bool isGameOver = false;
    public Score score;
    private Level currentLevel;

    private void OnEnable()
    {
        SlicesManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        SlicesManager.OnGameOver -= GameOver;
        isGameOver = false;
    }

    void Start()
    {
        currentLevel = LevelsManager.CurrentLevel;
        InitialiseLevel();
    }

    public void InitialiseLevel()
    {
        isGameOver = false;
        OnLevelInitialised.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        currentLevel.PlayingCount++;

        if (score.CurrentStars >= currentLevel.MinStarsToWin) //TODO: hardcoded winning condition(Can be moved to Level)
        {
            currentLevel.LevelSucceeded();
            OnWin?.Invoke(score.CurrentStars);
        }
        else
        {
            OnLose.Invoke();
        }
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
