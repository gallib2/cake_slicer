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
    public Level[] gameLevels; // TODO - maybe later we can read this from a config file
    private SaveAndLoadManager.LevelsSavedData savedData;
    private static bool areAllLevelsUnlocked = false;

    /*public void LoadLevel(int levelNumber)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        CurrentLevelNumber = levelNumber;
        CurrentLevel = gameLevels[levelNumber/*-1];
        CurrentLevel.PlayingCount++;
        SceneManager.LoadScene(cakesScene);
    }*/

    public void LoadLevel(int levelIndex)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
       // for (int i = 0; i < gameLevels.Length; i++)
        {
          //  if(level == gameLevels[i])
            {
                
                //Debug.Log("----------------- isPrevLevelSucceeded: " + isPrevLevelSucceeded);
                //Debug.Log("----------------- i: " + isPrevLevelSucceeded);
                /*if (isPrevLevelSucceeded)
                {
                    level.IsLocked = false;
                }*/
                bool isLocked = IsLevelLocked(levelIndex);//TODO: we do a for loop there, should we save level index on the levels themselves?
               // Debug.Log("level.IsLocked: " + level.IsLocked);
                if (!isLocked || areAllLevelsUnlocked)
                {
                    CurrentLevelNumber = levelIndex;
                    CurrentLevel = gameLevels[levelIndex];
                    CurrentLevel.PlayingCount++;
                    SceneManager.LoadScene(cakesScene);
                }
            }
        }
    }

    public bool IsLevelLocked(int levelIndex)
    {
        //for (int i = 0; i < gameLevels.Length; i++)
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
        //Debug.LogError("Level was not found!");
        //return false;
    }

    private void Awake()
    {
        savedData = SaveAndLoadManager.LoadLevelsSavedData();
    }

    public System.UInt32? GetLevelSavedScore(int levelIndex)
    {
        //for (int i = 0; i < gameLevels.Length; i++)
        {
            //if (level == gameLevels[i])
            {
                return savedData.savedLevelsData[levelIndex].score;
            }
        }
        //Debug.LogError("Level was not found!");
        return null;
    }


    public Level GetLevel(int levelIndex)
    {
           return gameLevels[levelIndex];
    }
}
