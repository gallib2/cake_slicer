using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeaderSetting : MonoBehaviour
{
    //public GameObject prefab;
    public GameObject[] prefabs;
    public Text goalText;

   // private int goal;

    private void OnEnable()
    {
      //  GameManager.OnNextLevel += NextLevel;
        SlicesManager.OnGoalChange += GoalChange;
    }

    private void OnDisable()
    {
       // GameManager.OnNextLevel -= NextLevel;
        SlicesManager.OnGoalChange -= GoalChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        // goal = GameManager.currentGoal;
        //goal = SlicesManager.goal;
    }

   /* private void GoalChange(int newGoal)
    {
        goalText.text = newGoal.ToString();
       // goal = SlicesManager.goal;
    }*/
    private void GoalChange()
    {
        // goalText.text = newGoal.ToString();
        //goal = SlicesManager.goal;
        goalText.text = SlicesManager.goal.ToString();
    }

    /*private void NextLevel()
    {
        //goalText.text = GameManager.currentGoal.ToString();
        goalText.text = SlicesManager.goal.ToString();
    }*/
}
