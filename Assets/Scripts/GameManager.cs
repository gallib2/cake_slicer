using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<int,bool> OnWin;
    public static event Action OnLose;
    public static event Action<int> OnGameOver;
    public static event Action OnLevelInitialised;
    public static event Action<bool> OnPauseChanged;

    public static PauseUIManager[] pauseUIs;
    public static PauseUIManager pauseUIsGeneral;
    public static PauseUIManager pauseUIsOnEnter;

    public static string playerName;//TODO: Remove
    public static bool isGameOver = false;
    public static bool toSetPauseOnEnterLevel = false;
    public static bool currentLevelIsUntouched;

    //ToDo: move these out of GaymeManager!
    [SerializeField] private GameObject areYouSureObject;
    [SerializeField] private ConfirmationButton yesButton;
    [SerializeField] private ConfirmationButton noButton;

    public static bool GameIsPaused
    {
        get
        {
            return gameIsPaused;
        }
    }
    private static bool gameIsPaused = false;
    public Score score;

   // public static bool FunSlicing = false;

    [SerializeField] private Level currentLevel;

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
        pauseUIs = GetComponents<PauseUIManager>();
        pauseUIsGeneral = pauseUIs[0];
        pauseUIsOnEnter = pauseUIs[1];
        if (LevelsManager.CurrentLevel != null)
        {
            currentLevel = LevelsManager.CurrentLevel;
        }

        InitialiseLevel();
        if(toSetPauseOnEnterLevel)
        {
            SetPauseOnEnterGame(true);
            toSetPauseOnEnterLevel = false;
        }

    }

    public void NextLevel()
    {
        int nextlevelNumber = LevelsManager.CurrentLevelNumber + 1;
        LevelsManager.instance.LoadLevel(nextlevelNumber, Score.score, true);
    }

    public void SetPause(bool to)
    {
        pauseUIsGeneral.ChangeState(to);
        gameIsPaused = to;
    }

    public void SetPauseOnEnterGame(bool to)
    {
        pauseUIsOnEnter.ChangeState(to);
        gameIsPaused = to;
    }

    public void RestartLevelFromPause()
    {
        if (currentLevelIsUntouched)
        {
            areYouSureObject.SetActive(true);
            noButton.ClickAction = (delegate () { areYouSureObject.SetActive(false); });
            yesButton.ClickAction = (delegate () 
            {
                currentLevelIsUntouched = false;
                SaveAndLoadManager.TrySaveLevelData(LevelsManager.CurrentLevelNumber, 0, false, true);
                InitialiseLevel();
                areYouSureObject.SetActive(false);
            });
        }
        else
        {
            InitialiseLevel();
        }
    }

    public void GoToHomeScreenFromPause()
    {
        if (currentLevelIsUntouched)
        {
            areYouSureObject.SetActive(true);
            noButton.ClickAction = (delegate () { areYouSureObject.SetActive(false); });
            yesButton.ClickAction = (delegate ()
            {
                SaveAndLoadManager.TrySaveLevelData(LevelsManager.CurrentLevelNumber, 0, false, true);
                UnloadScene();
            });
        }
        else
        {
            UnloadScene();
        }
    }

    public void InitialiseLevel()
    {
        SetPause(false);
        isGameOver = false;
        OnLevelInitialised.Invoke();
        pauseUIsGeneral.HidePauseScreen();
    }

    public void GameOver()
    {
        isGameOver = true;
        bool won = score.CurrentStars >= 1;
        //TODO: If we see that saving and oading slows the device, 
        //we can import the loaded data that was loaded previously and thus avoid loading inside TrySaveLevelData
        SaveAndLoadManager.TrySaveLevelData
             (LevelsManager.CurrentLevelNumber, (UInt32)Score.score, won, currentLevelIsUntouched);
        if (won) //TODO: hardcoded winning condition(Can be moved to Level)
        {
            OnWin?.Invoke(score.CurrentStars, currentLevelIsUntouched);//TODO: Record the number of stars or/and score if it's larger than it was previously
        }
        else
        {
            OnLose.Invoke();
        }
        OnGameOver?.Invoke(Score.score);
        currentLevelIsUntouched = false;
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
