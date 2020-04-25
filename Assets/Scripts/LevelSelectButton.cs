using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{


    [SerializeField] private int levelIndex;
    [SerializeField] private TMPro.TextMeshProUGUI indexText;
    [SerializeField] private Image lockedImage;
    [SerializeField] private Image starsImage;
    [SerializeField] private GameObject starsPannel;
    // [SerializeField] private Level level;
    //[SerializeField] private Text text;

    [SerializeField] private LevelsManager levelsManager;
    //TODO: if we see that all these refferences flood our RAM, we can easily get rid of em.

    private void Start()
    {
        Draw(false);
    }


    public void Draw( bool areAllLevelsUnlocked = false)
    {
        //TODO: I think this class knows too much for its own good

        bool isLocked = levelsManager.IsLevelLocked(levelIndex) && !areAllLevelsUnlocked;
        lockedImage.gameObject.SetActive(isLocked);
        starsPannel.SetActive(!isLocked);

        if (!isLocked)
        {
            System.UInt32? savedScore = levelsManager.GetLevelSavedScore(levelIndex);
            if (savedScore == null)
            {
                Debug.LogError("Something's wrong!");
                return;
            }
            Level level = levelsManager.GetLevel(levelIndex);
            indexText.text = levelIndex.ToString();
            // text.text = level.DisplayName + "\n SCORE " + savedScore.ToString() + "\n";
            starsImage.sprite = SpriteHolder.GetStarsSprite(level.GetNumberOfStars((int)savedScore));
        }
    }

    public void LoadLevel()
    {
        levelsManager.LoadLevel(levelIndex);//LevelsManager can be turned into a singleton
    }

}
