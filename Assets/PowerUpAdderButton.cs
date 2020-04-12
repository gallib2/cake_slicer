using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAdderButton : MonoBehaviour
{
    [SerializeField]
    private PowerUpTypes powerUp;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    public void Add()
    {
        //This code is intended for debugging 
        PlayerStats.AddToPowerUp(powerUp);
        PlayerInformationText.instance.UpdateText();
    }

    private void Start()
    {
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        text.text = "ADD ONE"+ "\n" + powerUp.ToString() + "\nFOR FREE";
    }
}
