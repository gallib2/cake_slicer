using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager: MonoBehaviour
{
    private const int cakesScene = 2;
    public static Level CurrentLevel { get; set; }
    public static int currentLevelNumber { get; set; }

    public Level[] gameLevels; // TODO - maybe later we can read this form a config file

    public void LoadLevel(int levelNumber)
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        currentLevelNumber = levelNumber;
        CurrentLevel = gameLevels[levelNumber-1];
        SceneManager.LoadScene(cakesScene);
    }
}
