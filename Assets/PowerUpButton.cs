using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField]
    private PowerUpTypes powerUp;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    public void UsePowerUp()
    {
        if (GameManager.GameIsPaused)
        {
            return;
        }
        if (PlayerStats.UsePowerUp(powerUp))
        {
            PowerUps.instance.UsePowerUp(powerUp);
            UpdateGraphics();
        }
    }

    private void Start()
    {
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        text.text = powerUp.ToString() + "\n" + PlayerStats.GetPowerUpUsesLeft(powerUp).ToString();
    }
}
