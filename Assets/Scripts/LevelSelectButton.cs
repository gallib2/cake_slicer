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
        System.UInt32 savedScore = levelsManager.GetLevelSavedScore(level);
        text.text = level.DisplayName+"\n SCORE "+ savedScore.ToString() + "\n";
        double ScoreDividedByMaxScore = ((double)savedScore / level.MaximumScore());
        for (int i = 0; i < level.StarRequirements.Length; i++)//TODO: copied from SetScore.. this piece of code should reside somewhere else
        {
            bool shouldGetStar = (ScoreDividedByMaxScore >= level.StarRequirements[i]);
            if (shouldGetStar)
            {
                text.text += (i == 0 ? "*":" *");
            }
        }
    }

    public void LoadLevel()
    {
        levelsManager.LoadLevel(level);//LevelsManager can be turned into a singleton
    }

}
