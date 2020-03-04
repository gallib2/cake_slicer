using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField]
    private Level level;
    [SerializeField]
    private Text text;
    [SerializeField]
    private LevelsManager levelsManager;

    void Start()
    {
        text.text = level.DisplayName;
    }

    public void LoadLevel()
    {
        levelsManager.LoadLevel(level);//LevelsManager can be turned into a singleton
    }

}
