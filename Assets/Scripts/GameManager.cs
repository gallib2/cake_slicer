﻿using System;
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
    public static event Action<int> OnGameOver;
    public static event Action OnLevelInitialised;

    public static string playerName;
    public static bool isGameOver = false;
    public Score score;

    public static bool FunSlicing = false;

    [SerializeField]
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
        if (score.CurrentStars >= 1/*currentLevel.MinStarsToWin*/) //TODO: hardcoded winning condition(Can be moved to Level)
        {
            currentLevel.LevelSucceeded();
            SaveAndLoadManager.TrySaveLevelData(LevelsManager.CurrentLevelNumber, (UInt32)Score.score);
            OnWin?.Invoke(score.CurrentStars);//TODO: Record the number of stars or/and score if it's larger than it was previously
        }
        else
        {
            OnLose.Invoke();
        }
        OnGameOver?.Invoke(Score.score);
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene(0);
    }

   /* public void SetFunSlicing(bool to)
    {
        FunSlicing = to;
    }*/
}
