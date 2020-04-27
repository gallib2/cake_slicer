using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager: MonoBehaviour
{
    private const int cakesScene = 1;
    public static Level CurrentLevel { get; set; }
    public static int CurrentLevelNumber { get; set; }
    public static event Action OnAllLevelsUnlocked;

    public static void SetLevelsUnlocked(bool to)
    {
        areAllLevelsUnlocked = to;
        //if (to)
        {
            // OnAllLevelsUnlocked();
            //This should have been an event but since this is a debugging tool, should we really care?
            var levelButtons = FindObjectsOfType<LevelSelectButton>();
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].Draw(to);
            }  
        }
    }

    //Should these not be static?
    [SerializeField]private Level[] gameLevels; // TODO - maybe later we can read this from a config file
    private SaveAndLoadManager.LevelsSavedData savedData;
    private static bool areAllLevelsUnlocked = false;

    public static LevelsManager instance;
    /*public void LoadLevel(int levelNumber)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        CurrentLevelNumber = levelNumber;
        CurrentLevel = gameLevels[levelNumber/*-1];
        CurrentLevel.PlayingCount++;
        SceneManager.LoadScene(cakesScene);
    }*/
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogWarning("Tried to create more than one singleton");
        }

        savedData = SaveAndLoadManager.LoadLevelsSavedData();
    }

    public int NumberOfLevels
    {
        get
        {
            return gameLevels.Length;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        //Debug.Log("----------------- isPrevLevelSucceeded: " + isPrevLevelSucceeded);
        //Debug.Log("----------------- i: " + isPrevLevelSucceeded);
        /*if (isPrevLevelSucceeded)
        {
            level.IsLocked = false;
        }*/
        if (IsLevelIndexLegit(levelIndex))
        {
            bool isLocked = IsLevelLocked(levelIndex);//TODO: we do a for loop there, should we save level index on the levels themselves?
                                                      // Debug.Log("level.IsLocked: " + level.IsLocked);
            if (!isLocked || areAllLevelsUnlocked)
            {
                CurrentLevelNumber = levelIndex;
                CurrentLevel = gameLevels[levelIndex];
                //CurrentLevel.PlayingCount++;
                SceneManager.LoadScene(cakesScene);
            }
        }
    }

    public bool IsLevelLocked(int levelIndex)
    {

        //for (int i = 0; i < gameLevels.Length; i++)
        if (IsLevelIndexLegit(levelIndex))
        {
            // if (level == gameLevels[i])
            {
                bool isLocked = true;
                bool isBeforeSecondLevel = levelIndex < 2;
                if (isBeforeSecondLevel)
                {
                    isLocked = false;
                }
                else
                {
                    bool isPreviousLevelSucceeded = gameLevels[levelIndex - 1].IsLevelComplete((int)GetLevelSavedScore(levelIndex - 1));
                    isLocked = !isPreviousLevelSucceeded;
                }          
                return isLocked;
            }
        }
        return true;
        //Debug.LogError("Level was not found!");
        //return false;
    }



    public System.UInt32? GetLevelSavedScore(int levelIndex)
    {
       if (IsLevelIndexLegit(levelIndex))
       {
            return savedData.savedLevelsData[levelIndex].score;
       }
        return null;
    }

    public LevelStates? GetLevelSavedState(int levelIndex)
    {
        if (IsLevelIndexLegit(levelIndex))
        {
            return savedData.savedLevelsData[levelIndex].state;
        }
        return null;
    }

    public Level GetLevel(int levelIndex)
    {
       if( IsLevelIndexLegit(levelIndex))
       {
            return gameLevels[levelIndex];

       }
       return null;
    }

    private bool IsLevelIndexLegit(int levelIndex)
    {
        if(levelIndex<0|| levelIndex > gameLevels.Length - 1)
        {
            Debug.LogError("Level Index is not legit!");
            return false;
        }
        return true;
    }

}
