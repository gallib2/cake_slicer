using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnWin;
    public static event Action OnLose;
    public static event Action<int> OnGameOver;
    public static event Action OnLevelInitialised;
    public static event Action<bool> OnPauseChanged;

    public static string playerName;
    public static bool isGameOver = false;
    public static bool GameIsPaused
    {
        get
        {
            return gameIsPaused;
        }
    }
    private static bool gameIsPaused = false;
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
        if (LevelsManager.CurrentLevel != null)
        {
            currentLevel = LevelsManager.CurrentLevel;
        }

        InitialiseLevel();
    }

    public void NextLevel()
    {
        int nextlevelNumber = LevelsManager.CurrentLevelNumber + 1;
        LevelsManager.instance.LoadLevel(nextlevelNumber, Score.score);
    }

    public void SetPause(bool to)
    {
        OnPauseChanged(to);
        gameIsPaused = to;
    }

    public void InitialiseLevel()
    {
        SetPause(false);
        isGameOver = false;
        OnLevelInitialised.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        bool won = score.CurrentStars >= 1;
        //TODO: If we see that saving and oading slows the device, 
        //we can import the loaded data that was loaded previously and thus avoid loading inside TrySaveLevelData
        SaveAndLoadManager.TrySaveLevelData(LevelsManager.CurrentLevelNumber, (UInt32)Score.score, won);
        if (won) //TODO: hardcoded winning condition(Can be moved to Level)
        {
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
}
