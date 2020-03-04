using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameDisplay : MonoBehaviour
{
    [SerializeField]
    private Text text;

    void Start()
    {
        text.text = LevelsManager.CurrentLevel.DisplayName;
    }
}
