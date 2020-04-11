using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Image timerFillImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    private float timeLeft;
    public bool ToStopTimer { get; set; }

    [SerializeField]
    private Level currentLevel;

    private void Awake()
    {
        if (LevelsManager.CurrentLevel != null)
        {
            currentLevel = LevelsManager.CurrentLevel;
        }

        GameManager.OnLevelInitialised += InitialiseLevel;
    }
    private void OnDisable()
    {
        GameManager.OnLevelInitialised -= InitialiseLevel;
    }

    private void InitialiseLevel()
    {
        ToStopTimer = false;
        timeLeft = currentLevel.InitialTimeInSeconds;
    }

    void Update()
    {
        if (GameManager.GameIsPaused)
        {
            return;
        }
        if (!ToStopTimer)
        {
            timeLeft -= Time.deltaTime;
            TimerGraphicsUpdate();

            if ( timeLeft < 0)
            {
                //GameOver();
                ToStopTimer = true;
            }
        }
    }

    public void AddTime(float timeToAdd)
    {
        timeLeft += timeToAdd;
    }

    private void TimerGraphicsUpdate()
    {
        text.text = (Mathf.CeilToInt( timeLeft)).ToString();
        //sliderTimer.value = (timeLeft / currentLevel.InitialTimeInSeconds);//This should work if slider max value's 1 and min value's 0 
        timerFillImage.fillAmount = (timeLeft / currentLevel.InitialTimeInSeconds);//This should work if slider max value's 1 and min value's 0 
        /*Color sliderColour;
        if (timerFillImage.fillAmount > 0.5f)
        {
            sliderColour = Color.Lerp(Color.yellow, Color.green, timerFillImage.fillAmount - 0.5f);
        }
        else
        {
            sliderColour = Color.Lerp(Color.red, Color.yellow, timerFillImage.fillAmount + 0.5f);
        }

        timerFillImage.color = sliderColour;*/
        //  sliderTimer.value =(int)timer.Get();
        /*if (timeLeft <= criticalTime)
        {
            timerAnimation.SetBool("isCritical2", true);
        }
        bool isAnimOn = timerAnimation.GetBool("isCritical2");
        if (isAnimOn && timeLeft > criticalTime)
        {
            timerAnimation.SetBool("isCritical2", false);
        }*/

    }
}
