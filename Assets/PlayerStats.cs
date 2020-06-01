using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public string name;
    public UInt32 lives;
    public Dictionary<PowerUpTypes, UInt32> powerUps;
    public DateTime lastExtraLifeDateTime;

    public PlayerData() { }

    public PlayerData
        (string name, UInt32 lives, Dictionary<PowerUpTypes, UInt32> powerUps, DateTime lastExtraLifeDateTime)
    {
        this.name = name;
        this.lives = lives;
        this.powerUps = powerUps;
        this.lastExtraLifeDateTime = lastExtraLifeDateTime;
    }
}

public static class PlayerStats 
{
    private static PlayerData data;
    private const int SECONDS_TO_GAIN_AN_EXTRA_LIFE = 10;
    private const UInt32 MAX_LIVES = 7;
    public static event Action<uint> OnLivesChanged;

    #region Getters:
    public static string Name
    {
        get { return data.name; }
    }

    public static UInt32 Lives
    {
        get {  return data.lives; }
    }

    public static Dictionary<PowerUpTypes, UInt32> PowerUps
    {
        get { return data.powerUps; }
    }
    #endregion

    /* {
         get
         {
             if ( == null)
             {
                 InitialisePowerUps();
             }
             return powerUps;
         }
         set{ }
     }*/
    #region PowerUps:
    public static bool UsePowerUp(PowerUpTypes powerUp)
    {
        UInt32 usesLeft;
        if (data.powerUps.TryGetValue(powerUp, out usesLeft))
        {
            if (usesLeft > 0)
            {
                data.powerUps[powerUp]--;
                SaveChanges();
                return true;
            }
            Debug.Log("The powerup you're lookin' for is empty");
        }
        else
        {
            Debug.Log("The powerup you're lookin' for weren't found in the dictionary");
        }
        return false;
    }


    public static void AddToPowerUp(PowerUpTypes powerUp)
    {
        //This code is intended for debugging 

        if (data.powerUps == null)
        {
            Initialise();
        }
        data.powerUps[powerUp]++;
        SaveChanges();
    }

    public static UInt32? GetPowerUpUsesLeft(PowerUpTypes powerUp)
    {
        UInt32? usesLeft = null;
        if (data.powerUps.ContainsKey(powerUp))
        {
            usesLeft = data.powerUps[powerUp];
        }
        return usesLeft;
    }

    #endregion
    public static void AddLives(UInt32 livesToAdd)
    {
        // bounds(?) check:
        UInt32 newNumberOfLives = data.lives + livesToAdd;
        if(newNumberOfLives > MAX_LIVES || newNumberOfLives < data.lives)
        {
            newNumberOfLives = MAX_LIVES;
        }
        data.lives = newNumberOfLives;
        SaveChanges();
        OnLivesChanged(data.lives);
        Debug.Log("Lives: " + data.lives);

    }

    public static void RemoveLives(UInt32 livesToRemove)
    {
        if (data.lives - livesToRemove > data.lives)
        {
            data.lives = 0;
            Debug.LogWarning("Tried to remove more lives than we have");
        }
        else
        {
            if(data.lives >= MAX_LIVES)
            {
                Debug.Log("Was at max lives");
                data.lastExtraLifeDateTime = DateTime.Now;
                //TODO: start doing timed checks
            }
            data.lives -= livesToRemove;
        }
        SaveChanges();
        OnLivesChanged(data.lives);
        Debug.Log("Lives: " + data.lives);
    }

    public static void UpdateDateTime()
    {
        if (data.lives >= MAX_LIVES)
        {
            Debug.Log("At max lives");
            return;
        }
        DateTime now = DateTime.Now;
        DateTime lastExtraLifeDateTime = data.lastExtraLifeDateTime;
        TimeSpan timeSinceLastExtraLife = now - lastExtraLifeDateTime;

        int livesOwed = (int)timeSinceLastExtraLife.TotalSeconds / SECONDS_TO_GAIN_AN_EXTRA_LIFE;//TODO: check accuracy
        if (livesOwed > 0)
        {
            Debug.Log("+<3+");
            data.lastExtraLifeDateTime = lastExtraLifeDateTime.AddSeconds(livesOwed * SECONDS_TO_GAIN_AN_EXTRA_LIFE);
            AddLives((UInt32)livesOwed);
        }
        else
        {
            Debug.Log("timeSinceLastExtraLife.TotalSeconds :" + (int)timeSinceLastExtraLife.TotalSeconds);

        }
        //TODO: save?
    }

    public static void Initialise()
    {
        PlayerData savedData = LoadSavedData();
        if (savedData != null)
        {
            data = savedData;
            SaveChanges();//TODO: why is this here?
        }
        else
        {
            data = new PlayerData("MRS CORONA", 6, new Dictionary<PowerUpTypes, uint>(), DateTime.Now);
            data.powerUps.Add(PowerUpTypes.EXTRA_TIME, 2);
            data.powerUps.Add(PowerUpTypes.GOLDEN_KNIFE, 0);
            data.powerUps.Add(PowerUpTypes.IMMUNITY, 2);
            data.powerUps.Add(PowerUpTypes.WHIPPED_CREAM, 0);
        }

        GameManager.OnLose+= (delegate()
        {
            RemoveLives(1);
        });

        UpdateDateTime();
    }

    public static void SaveChanges()
    {
        SaveAndLoadManager.SavePlayerData(data);
    }

    public static PlayerData LoadSavedData()
    {
       return SaveAndLoadManager.LoadPlayerSavedData();
    }
}
