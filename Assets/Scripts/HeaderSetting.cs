using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeaderSetting : MonoBehaviour
{
    public GameObject[] prefabs;
    public Text goalText;

    private void OnEnable()
    {
        SlicesManager.OnGoalChange += GoalChange;
    }

    private void OnDisable()
    {
        SlicesManager.OnGoalChange -= GoalChange;
    }


    private void GoalChange()
    {
        goalText.text = SlicesManager.goal.ToString();
    }
}
