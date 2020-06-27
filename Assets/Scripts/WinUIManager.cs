using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUIManager : MonoBehaviour
{
    [SerializeField] private Sprite emptyStarImage;
    [SerializeField] private Sprite fullStarImage;
    [SerializeField] private WinPopUpStar[] stars;
    [SerializeField] private float fillStarsWait = 0.5f;
    [SerializeField] private GameObject[] elementsToAppearOnWin;
    [SerializeField] private GameObject[] elementsToDisppearOnWin;
    [SerializeField] private GameObject firstTryPopUp;
    [SerializeField] private GameObject finalScoreAndStarsPopUp;
    //bookeeping:
    private  int numberOfStars;
    private float firstTryPopUpTimer;
    private bool isShowingfirstTryPopUp;

    private void Awake()
    {
        GameManager.OnWin += ShowWinScreen;
        GameManager.OnLevelInitialised += HideWinScreen;
       // HideWinScreen();//Should be called by OnLevelInitialised though
    }

    private void OnDisable()
    {
        GameManager.OnWin -= ShowWinScreen;
        GameManager.OnLevelInitialised -= HideWinScreen;
    }

    private void HideWinScreen()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].EmptyStar();
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

    private void ShowWinScreen(int numberOfStars, bool isFirstTry)
    {
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnWin.Length; i++)
        {
            elementsToDisppearOnWin[i].SetActive(false);
        }

        if (isFirstTry)
        {
            ShowFirstTryPopUp(numberOfStars);
        }
        else
        {
            ShowFinalScoreAndStarsPopUp(numberOfStars);
        }
    }

    private void ShowFirstTryPopUp(int numberOfStars)
    {
        firstTryPopUp.SetActive(true);
        finalScoreAndStarsPopUp.SetActive(false);
        this.numberOfStars = numberOfStars;
        firstTryPopUpTimer = 3f;
        isShowingfirstTryPopUp = true;
    }

    private void Update()
    {
        if(isShowingfirstTryPopUp)
        {
            firstTryPopUpTimer -= Time.deltaTime;
            if (firstTryPopUpTimer < 0 || InputManager.GetTouchDown())
            {
                ShowFinalScoreAndStarsPopUp(numberOfStars);
                isShowingfirstTryPopUp = false;
            }
        }
    }

    private void ShowFinalScoreAndStarsPopUp(int numberOfStars)
    {
        firstTryPopUp.SetActive(false);
        finalScoreAndStarsPopUp.SetActive(true);
        StartCoroutine(FillStars(numberOfStars));
    }

    public IEnumerator FillStars(int numberOfStars)
    {
        Debug.Log("numberOfStars: " + numberOfStars);
        for (int i = 0; i < stars.Length; i++)
        {
            yield return new WaitForSeconds(fillStarsWait);
            bool toFillStars = numberOfStars >= i + 1;//TODO: do we have to ask this? we know the player wins only when at least one star was collected

            if (toFillStars)
            {
                //FillStar(stars[i]);
                stars[i].FillStar();
            }
        }
    }

}
