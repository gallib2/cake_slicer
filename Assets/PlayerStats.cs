using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public static class PlayerStats 
{
    private static string name;
    private static UInt32 lives;
    private static Dictionary<PowerUpTypes, UInt32> powerUps;
    #region Getters:
    public static string Name
    {
        get
        {
            return name;
        }
    }
    public static UInt32 Lives
    {
        get
        {
            return lives;
        }
    }
    public static Dictionary<PowerUpTypes, UInt32> PowerUps
    {
        get
        {
            return powerUps;
        }
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

    public static bool UsePowerUp(PowerUpTypes powerUp)
    {
        UInt32 usesLeft;
        if (powerUps.TryGetValue(powerUp, out usesLeft))
        {
            if (usesLeft > 0)
            {
                powerUps[powerUp]--;
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

        if (powerUps == null)
        {
            Initialise();
        }
        powerUps[powerUp]++;
        SaveChanges();
    }

    public static UInt32? GetPowerUpUsesLeft(PowerUpTypes powerUp)
    {
        UInt32? usesLeft = null;
        if (powerUps.ContainsKey(powerUp))
        {
            usesLeft = powerUps[powerUp];
        }
        return usesLeft;
    }

    public static void Initialise()
    {
        SaveAndLoadManager.PlayerSavedData savedData = LoadSavedData();
        if (savedData != null)
        {
            name = savedData.name;
            lives = savedData.lives;
            powerUps = savedData.powerUps;
            SaveChanges();
        }
        else
        {
            name = "MRS CORONA";
            lives = 6;
            powerUps = new Dictionary<PowerUpTypes, uint>();
            powerUps.Add(PowerUpTypes.EXTRA_TIME, 2);
            powerUps.Add(PowerUpTypes.GOLDEN_KNIFE, 0);
            powerUps.Add(PowerUpTypes.IMMUNITY, 2);
            powerUps.Add(PowerUpTypes.WHIPPED_CREAM, 0);
        }
    }

    public static void SaveChanges()
    {
        SaveAndLoadManager.SavePlayerData(name, lives, powerUps);
    }

    public static SaveAndLoadManager.PlayerSavedData LoadSavedData()
    {
       return SaveAndLoadManager.LoadPlayerSavedData();
    }
}
