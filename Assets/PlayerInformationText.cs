using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationText : MonoBehaviour
{
    public static PlayerInformationText instance;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

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
        PlayerStats.Initialise();//TODO: This is absolutely not the proper place for the player to initialise.
        UpdateText();
    }

    public void UpdateText()
    {
        Dictionary<PowerUpTypes, System.UInt32> powerUps = PlayerStats.PowerUps;
        text.text =
            "Welcome, " + PlayerStats.Name + "\n" +
            "You have got: " + "\n"
            + PlayerStats.Lives + " lives" + "\n";
        //It's all dumb, I know..
        for (int i = 0; i < (int)PowerUpTypes.Length; i++)
        {
            text.text += powerUps[(PowerUpTypes)i].ToString() + " " + ((PowerUpTypes)i).ToString() + "\n";
        }
    }

    private float lastPlayerUpdateDateTimeTime = 0;
    private void Update()
    {
        //TODO: This should obviously not stay here.
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerStats.UpdateDateTime();
        }
        if(Time.time - 1 > lastPlayerUpdateDateTimeTime)
        {
            lastPlayerUpdateDateTimeTime = Time.time;
            PlayerStats.UpdateDateTime();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            PlayerStats.RemoveLives(1);
        }
        if (Input.GetKeyDown(KeyCode.Plus))
        {
            PlayerStats.AddLives(1);
        }
    }
}
