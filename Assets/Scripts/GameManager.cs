using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public static event Action OnGameOver;
    public static event Action OnWin;
    public static event Action OnLose;
    public static event Action k;
    public static string playerName;

    //TimerHelper timer;
    //float timerRequired = 1f;

    public List<int> slicesSizeList;
    //[SerializeField]
   //public  int score = 0;// Ori finds this variable unnesssry!
    public static bool isGameOver = false;

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
        isGameOver = false;
    }

    private void StartGameSettings()
    {
        //Score.ins = 0;
    }

    public static void GameOver()
    {
        isGameOver = true;
        //OnGameOver?.Invoke();

        if (Score.hasStarAt[0])//TODO: hardcoded winning condition(Can be moved to Level)
        {
            OnWin.Invoke();
        }
        else
        {
            OnLose.Invoke();
        }
    }
}
