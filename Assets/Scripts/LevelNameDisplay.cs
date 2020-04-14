using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    void Start()
    {
        if (LevelsManager.CurrentLevel != null)
        {
            text.text = LevelsManager.CurrentLevel.DisplayName;
        }
       
    }
}
