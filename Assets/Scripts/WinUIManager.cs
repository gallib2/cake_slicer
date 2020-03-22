using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUIManager : MonoBehaviour
{
    [SerializeField]
    private Sprite emptyStarImage;
    [SerializeField]
    private Sprite fullStarImage;
    [SerializeField]
    private WinPopUpStar[] stars;
    [SerializeField]
    private float fillStarsWait = 0.5f;
    [SerializeField]
    private GameObject[] elementsToAppearOnWin;
    [SerializeField]
    private GameObject[] elementsToDisppearOnWin;
    [SerializeField]
    private GameOverScoreDisplay scoreDisplay;

   private void Awake()
    {
        GameManager.OnWin += ShowFinalScoreAndStars;
        GameManager.OnLevelInitialised += HideWinScreen;
       // HideWinScreen();//Should be called by OnLevelInitialised though
    }

    private void HideWinScreen()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            // if (Score.hasStarAt[i])
            //{
            //EmptyStar(stars[i]);
            stars[i].EmptyStar();
            //}
        }
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(false);
        }
        for (int i = 0; i < elementsToDisppearOnWin.Length; i++)
        {
            elementsToDisppearOnWin[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        GameManager.OnWin -= ShowFinalScoreAndStars;
        GameManager.OnLevelInitialised -= HideWinScreen;
    }

    private void ShowFinalScoreAndStars(int numberOfStars)
    {
        scoreDisplay.SetText(Score.score.ToString());//Turn into an event maybe?
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnWin.Length; i++)
        {
            elementsToDisppearOnWin[i].SetActive(false);
        }
        StartCoroutine(FillStars(numberOfStars));
    }

    public IEnumerator FillStars(int numberOfStars)
    {
        Debug.Log("numberOfStars: " + numberOfStars);
        for (int i = 0; i < stars.Length; i++)
        {
            yield return new WaitForSeconds(fillStarsWait);
            bool toFillStars = numberOfStars >= i + 1;

            if (toFillStars)
            {
                //FillStar(stars[i]);
                stars[i].FillStar();
            }
        }
    }

    /*private void FillStar(Image star)
    {
        star.sprite = fullStarImage;
    }

    private void EmptyStar(Image star)
    {
        star.sprite = emptyStarImage;
    }*/
}
