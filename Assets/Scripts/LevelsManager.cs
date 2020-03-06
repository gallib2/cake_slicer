using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager: MonoBehaviour
{
    private const int cakesScene = 1;
    public static Level CurrentLevel { get; set; }
    public static int CurrentLevelNumber { get; set; }

    public Level[] gameLevels; // TODO - maybe later we can read this from a config file

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
                CurrentLevelNumber = i;
                CurrentLevel = level;
                CurrentLevel.PlayingCount++;
                SceneManager.LoadScene(cakesScene);
            }
        }
    }
}
