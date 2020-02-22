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
    [SerializeField]
    public Level currentLevel;
    public Score score;

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

        if (score.CurrentStars > 1) //TODO: hardcoded winning condition(Can be moved to Level)
        {
            OnWin?.Invoke(score.CurrentStars);
        }
        else
        {
            OnLose.Invoke();
        }
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(1);
    }
}
