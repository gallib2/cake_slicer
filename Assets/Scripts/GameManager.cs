using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnNextLevel;
    public static event Action OnGameOver;

    public delegate void GoalChange(int goal);

    public static event GoalChange OnGoalChange;
    public static string playerName;

    //TimerHelper timer;
    //float timerRequired = 1f;

    public List<int> slicesSizeList;
    public static List<int> goals = new List<int>();
    public static int currentGoal;
    public static int score = 0;
    public static bool isGameOver = false;

    private static int minSliceGoal = 2;
    private static int maxSliceGoal = 6;

    public static int currentLevel = 0;

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
        score = 0;
        isGameOver = false;
        goals.Clear();
    }

    private void StartGameSettings()
    {
        timer = TimerHelper.Create();
        goals.Add(2);
        currentGoal = goals.Last();
        score = 0;
        currentLevel = 0;
        maxSliceGoal = 6;
        OnNextLevel?.Invoke();
        OnGoalChange?.Invoke(currentGoal);
    }

    // TODO event
    public static void NextLevel()
    {
        int nextGoal = GetNextGoal();

        goals.Add(nextGoal);

        currentGoal = goals.Last();
        score = goals.Count - 1;
        currentLevel++;

        OnNextLevel?.Invoke();

        OnGoalChange?.Invoke(currentGoal);
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
        goals.Clear();
        
        StartGameSettings();


        //if (gameOverScreenPrefub != null)
        //{
        //    Destroy(gameOverScreenPrefub.gameObject);
        //}
    }

    // goal need to be even
    private static int GetNextGoal()
    {
        int goal = UnityEngine.Random.Range(minSliceGoal, maxSliceGoal);
        bool isGoalOdd = goal % 2 != 0;
        if (isGoalOdd)
        {
            goal += 1;
        }

        Debug.Log("currentLevel: " + currentLevel);

        if(maxSliceGoal < 8 && (currentLevel % 5 == 0))
        {
            maxSliceGoal += 2; 
        }

        return goal;
    }
}
