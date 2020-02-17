using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnNextLevel;
    public static event Action OnGameOver;
    public static string playerName;

    //TimerHelper timer;
    //float timerRequired = 1f;

    public List<int> slicesSizeList;
    //[SerializeField]
   //public  int score = 0;// Ori finds this variable unnesssry!
    public static bool isGameOver = false;

    TimerHelper timer;
    static public GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //timer = TimerHelper.Create();

        StartGameSettings();
    }

    private void OnDisable()
    {
       // score = 0;
        isGameOver = false;
    }

    private void StartGameSettings()
    {
        timer = TimerHelper.Create();
       // score = 0;
        OnNextLevel?.Invoke();
    }

    // TODO event
    public static void NextLevel()
    { 
       // instance.score = goals.Count - 1;

        OnNextLevel?.Invoke();
    }

    public static void GameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
        //OnNextLevel?.Invoke();
    }

    public void PlayAgain()
    {
        isGameOver = false;
       // goals.Clear();
        
        StartGameSettings();

        //if (gameOverScreenPrefub != null)
        //{
        //    Destroy(gameOverScreenPrefub.gameObject);
        //}
    }
}
