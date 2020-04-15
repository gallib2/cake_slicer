using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpTypes
{
    EXTRA_TIME = 0, GOLDEN_KNIFE = 1,SHOW_PERFECT_CUT = 2, IMMUNITY = 3, Length=4,
}

public class PowerUps : MonoBehaviour
{
    public static PowerUps instance;
    [Header("Extra Time")]
    [SerializeField]
    private float timeToAdd = 5f;
    [SerializeField]
    private Timer timer;
    [Header("Golden Knife")]
    [SerializeField]
    private float goldenKnifeInitialTime = 5f;
    public static bool goldenKnifeIsActive
    {
        get; private set;
    }
    private float goldenKnifeIsTimeLeft;


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
            case PowerUpTypes.EXTRA_TIME:
                ExtraTime();break;
            case PowerUpTypes.GOLDEN_KNIFE:
                GoldenKnife(); break;
        }
        
    }

    private void ExtraTime()
    {
        timer.AddTime(timeToAdd);
    }

    private void GoldenKnife()
    {
        Debug.Log("Golden Knife is active!");

        goldenKnifeIsActive = true;
        goldenKnifeIsTimeLeft = goldenKnifeInitialTime;
    }

    private void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            if (goldenKnifeIsTimeLeft > 0)
            {
                if (!goldenKnifeIsActive)
                {
                    Debug.LogError("Golden knife is not active despite goldenKnifeIsTimeLeft being larger than 0!");
                }
                goldenKnifeIsTimeLeft -= Time.deltaTime;
                if(goldenKnifeIsTimeLeft <= 0)
                {
                    goldenKnifeIsActive = false;
                    Debug.Log("Golden Knife aint active no more!");
                }
            }
        }
    }

}
