using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private bool addLVL = false;
    void Start()
    {
        if (LevelsManager.CurrentLevel != null)
        {
         // text.text = LevelsManager.CurrentLevel.DisplayName;
            text.text = (addLVL?"LVL ":"") +  LevelsManager.CurrentLevelNumber.ToString();
        }
    }
}
