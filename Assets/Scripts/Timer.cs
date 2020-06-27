using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image timerFillImage;
    [SerializeField] private TMPro.TextMeshProUGUI text;
   // [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator timerAnimator;


    private float timeLeft;
    private int timeLeftInt;
    public bool ToStopTimer { get; set; }

    [SerializeField] private Level currentLevel;
    [SerializeField] private int criticalTime = 10;
    [SerializeField] private Color startFillColour;
    [SerializeField] private Color midFillColour;
    [SerializeField] private Color finalFillColour;
    [SerializeField] private Color frozenFillColour;

    private void Awake()
    {
        if (LevelsManager.CurrentLevel != null)
        {
            currentLevel = LevelsManager.CurrentLevel;
        }

        GameManager.OnLevelInitialised += InitialiseLevel;
        PowerUps.OnTimeFrozen += FreezeTime;
        PowerUps.OnTimeUnfrozen += UnfreezeTime;

    }
    private void OnDisable()
    {
        GameManager.OnLevelInitialised -= InitialiseLevel;
        PowerUps.OnTimeFrozen -= FreezeTime;
        PowerUps.OnTimeUnfrozen -= UnfreezeTime;
    }

    private void InitialiseLevel()
    {
        ToStopTimer = false;
        timeLeft = currentLevel.InitialTimeInSeconds;
        TimerGraphicsUpdate();

    }

    private void FreezeTime()
    {
        timerFillImage.color = frozenFillColour;
        timerAnimator.SetTrigger("Freeze");
        timerAnimator.SetBool("IsFrozen", true);

    }

    private void UnfreezeTime()
    {
        timerAnimator.SetTrigger("Unfreeze");
        timerAnimator.SetBool("IsFrozen",false);

    }

    void Update()
    {
        if (GameManager.GameIsPaused)
        {
            return;
        }
        if (!ToStopTimer && !PowerUps.TimeIsFrozen)
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
        int timeLeftInt = Mathf.CeilToInt(timeLeft);
        bool timeLeftIntChanged = timeLeftInt != this.timeLeftInt;
        this.timeLeftInt = timeLeftInt;

        text.text = timeLeftInt.ToString();
        //sliderTimer.value = (timeLeft / currentLevel.InitialTimeInSeconds);//This should work if slider max value's 1 and min value's 0 
        timerFillImage.fillAmount = (timeLeft / currentLevel.InitialTimeInSeconds);//This should work if slider max value's 1 and min value's 0 
        /* Color timerFillColour = defaultFillColour;
         if(timeLeftInt <= criticalTime)
         {
             timerFillColour = criticalTimeFillColour;
         }*/

        Color timerFillColour;// = startFillColour;

        if(timeLeft > criticalTime)
        {

            float normaliser = ((timeLeft - criticalTime) /( currentLevel.InitialTimeInSeconds - criticalTime)) * 2;
            if (normaliser > 1)
            {
                timerFillColour = Color.Lerp(midFillColour, startFillColour, normaliser - 1);
            }
            else
            {
                timerFillColour = Color.Lerp(finalFillColour, midFillColour, normaliser);
            }
        }
        else
        {
            if (timeLeftIntChanged)
            {
                timerAnimator.SetTrigger("Pop");
            }
            timerFillColour = finalFillColour;
        }

        timerFillImage.color = timerFillColour;

        /*if (timerFillImage.fillAmount > 0.5f)
        {
            timerFillColour = Color.Lerp(midFillColour, startFillColour, timerFillImage.fillAmount - 0.5f);
        }
        else
        {
            timerFillColour = Color.Lerp(finalFillColour, midFillColour, timerFillImage.fillAmount *2);
        }*/

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
