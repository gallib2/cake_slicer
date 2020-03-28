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
    private float time = 5f;

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
        }
    }

    private void ExtraTime()
    {
        timer.AddTime(timeToAdd);
    }

}
