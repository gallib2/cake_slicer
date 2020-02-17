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
    private Image[] stars;
    [SerializeField]
    private float fillStarsWait = 0.5f;
    [SerializeField]
    private GameObject[] elementsToAppearOnWin;
    [SerializeField]
    private GameObject[] elementsToDisppearOnWin;

    private void Start()
    {
        GameManager.OnWin += ShowFinalScoreAndStars;
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(false);
        }
       
        for (int i = 0; i < stars.Length; i++)
        {
            if (Score.hasStarAt[i])
            {
                EmptyStar(stars[i]);
            }
        }
    }
    private void OnDisable()
    {
        GameManager.OnWin -= ShowFinalScoreAndStars;
    }
    private void ShowFinalScoreAndStars()
    {
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnWin.Length; i++)
        {
            elementsToDisppearOnWin[i].SetActive(false);
        }
        StartCoroutine(FillStars());
    }
    public IEnumerator FillStars()
    {
        if(Score.hasStarAt.Length!= stars.Length)
        {
            Debug.Log("Uneven lengths!!!");
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                yield return new WaitForSeconds(fillStarsWait);
                if (Score.hasStarAt[i])
                {
                    FillStar(stars[i]);
                }
            }
        }
    }

    private void FillStar(Image star)
    {
        star.sprite = fullStarImage;
    }
    private void EmptyStar(Image star)
    {
        star.sprite = emptyStarImage;
    }
}
