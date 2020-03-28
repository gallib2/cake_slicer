using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager: MonoBehaviour
{
    private const int cakesScene = 1;
    public static Level CurrentLevel { get; set; }
    public static int CurrentLevelNumber { get; set; }
    //Should these not be static?
    public Level[] gameLevels; // TODO - maybe later we can read this from a config file
    private SavedData savedData;
    public static bool areAllLevelsUnlocked = false;

    public void LoadLevel(int levelNumber)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        CurrentLevelNumber = levelNumber;
        CurrentLevel = gameLevels[levelNumber/*-1*/];
        CurrentLevel.PlayingCount++;
        SceneManager.LoadScene(cakesScene);
    }

    public void LoadLevel(Level level)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        for (int i = 0; i < gameLevels.Length; i++)
        {
            if(level == gameLevels[i])
            {
                bool isBeforeSecondLevel = i < 2;
                bool isPrevLevelSucceeded = isBeforeSecondLevel || 
                    gameLevels[i - 1].IsLevelComplete((int)GetLevelSavedScore(gameLevels[i - 1]));
                Debug.Log("----------------- isPrevLevelSucceeded: " + isPrevLevelSucceeded);
                //Debug.Log("----------------- i: " + isPrevLevelSucceeded);
                /*if (isPrevLevelSucceeded)
                {
                    level.IsLocked = false;
                }*/
                bool isLocked = !isPrevLevelSucceeded;
               // Debug.Log("level.IsLocked: " + level.IsLocked);
                if (!isLocked || areAllLevelsUnlocked)
                {
                    CurrentLevelNumber = i;
                    CurrentLevel = level;
                    CurrentLevel.PlayingCount++;
                    SceneManager.LoadScene(cakesScene);
                }
            }
        }
    }

    private void Awake()
    {
        savedData = SaveAndLoadManager.LoadSavedData();
    }

    public System.UInt32? GetLevelSavedScore(Level level)
    {
        for (int i = 0; i < gameLevels.Length; i++)
        {
            if (level == gameLevels[i])
            {
                return savedData.savedLevelsData[i].score;
            }
        }
        Debug.Log("Level was not found!");
        return null;
    }
}
