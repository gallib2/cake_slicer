using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpTypes
{
    FREEZE_TIME = 0, //TODO: Change to freeze time
    GOLDEN_KNIFE = 1,
    WHIPPED_CREAM = 2,
    IMMUNITY = 3,
    Length =4,
}

public class PowerUps : MonoBehaviour
{
    public static PowerUps instance;
    [Header("Freeze Time")]
    [SerializeField] private float timeFreezeDurationInSeconds = 7f;
    public static bool TimeIsFrozen
    {
        get { return remainingFrozenTimeInSeconds > 0;}
    }
    private static float remainingFrozenTimeInSeconds = 0;
    public static event Action OnTimeFrozen;
    public static event Action OnTimeUnfrozen;
    //[SerializeField] private Timer timer;
    [Header("Golden Knife")]
    [SerializeField] private float goldenKnifeInitialTime = 5f;
    public static bool GoldenKnifeIsActive
    {
        get; private set;
    }
    private float goldenKnifeTimeLeft;
    public static event Action OnGoldenKnifeActivated;
    public static event Action OnGoldenKnifeDeactivated;

    //[Header("WhippedCream")]
    public static event Action OnWhippedCream;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UsePowerUp(PowerUpTypes powerUp)
    {
        Debug.Log("UsePowerUp: " + powerUp.ToString());
        switch (powerUp)
        {
            case PowerUpTypes.FREEZE_TIME:
                FreezeTime();break;
            case PowerUpTypes.GOLDEN_KNIFE:
                GoldenKnife(); break;
            case PowerUpTypes.WHIPPED_CREAM:
                WhippedCream(); break;
        }
        
    }

    private void FreezeTime()
    {
        //timer.AddTime(timeToAdd);
        OnTimeFrozen();
        remainingFrozenTimeInSeconds = timeFreezeDurationInSeconds;
    }

    private void GoldenKnife()
    {
        Debug.Log("Golden Knife is active!");
        GoldenKnifeIsActive = true;
        goldenKnifeTimeLeft = goldenKnifeInitialTime;
        OnGoldenKnifeActivated();
    }

    private void WhippedCream()
    {
        Debug.Log("Whippin'!");
        OnWhippedCream();
    }

    private void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            if (goldenKnifeTimeLeft > 0)
            {
                if (!GoldenKnifeIsActive)
                {
                    Debug.LogError("Golden knife is not active despite goldenKnifeIsTimeLeft being larger than 0!");
                }
                goldenKnifeTimeLeft -= Time.deltaTime;
                if(goldenKnifeTimeLeft <= 0)
                {
                    GoldenKnifeIsActive = false;
                    OnGoldenKnifeDeactivated();
                    Debug.Log("Golden Knife aint active no more!");
                }
            }

            if (remainingFrozenTimeInSeconds > 0)
            {
                remainingFrozenTimeInSeconds -= Time.deltaTime;
                if(remainingFrozenTimeInSeconds <= 0)
                {
                    OnTimeUnfrozen();
                }
            }
        }
    }

}
