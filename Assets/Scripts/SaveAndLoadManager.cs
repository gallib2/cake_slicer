
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;


public static class SaveAndLoadManager 
{
    #region Levels:

    [Serializable]
    public class LevelsSavedData
    {
        [Serializable]
        public class SavedLevelData
        {
            public UInt32 score;

            public SavedLevelData(UInt32 score)
            {
                this.score = score;
            }
        }
        public SavedLevelData[] savedLevelsData;

        public LevelsSavedData() { }

        public LevelsSavedData(int numberOfLevels)
        {
            savedLevelsData = new SavedLevelData[numberOfLevels];
            for (int i = 0; i < savedLevelsData.Length; i++)
            {
                savedLevelsData[i] = new SavedLevelData(0);
            }
        }
    }

    public static string BuildLevelsSaveFileName()
    {
        return (Application.persistentDataPath + "/levels_save" + ".dat");
    }

    public static LevelsSavedData LoadLevelsSavedData()
    {
        string fileName = BuildLevelsSaveFileName();
        if (File.Exists(fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(fileName, FileMode.Open);
            LevelsSavedData savedData = new LevelsSavedData();
            savedData = (LevelsSavedData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loading from " + fileName);
            return savedData;
        }
        else
        {
            return new LevelsSavedData(11);//TODO: Should not stay hardcoded...
        }
       // return null;
    }

    public static void SaveLevelsData(LevelsSavedData savedData)
    {
        string fileName = BuildLevelsSaveFileName();
        if (!File.Exists(fileName))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
        BinaryFormatter BF = new BinaryFormatter();
        FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
        BF.Serialize(fileStream, savedData);
        Debug.Log("Saving to " + fileName);
        fileStream.Close();
    }

    public static void TrySaveLevelData(int levelIndex, UInt32 newScore)
    {
        LevelsSavedData loadedSavedData = LoadLevelsSavedData();
        UInt32 oldScore = loadedSavedData.savedLevelsData[levelIndex].score;
        if (oldScore < newScore)
        {
            loadedSavedData.savedLevelsData[levelIndex].score = newScore;
            Debug.Log(oldScore + " is  smaller than " + newScore + ". Saving!");
            SaveLevelsData(loadedSavedData);
        }
        else
        {
            Debug.Log(oldScore+" is not smaller than "+ newScore+ ". No need to save");
        }
    }
    #endregion
    #region Player:
    [Serializable]
    public class PlayerSavedData
    {
        public string name;
        public UInt32 lives;
        public Dictionary<PowerUpTypes, UInt32> powerUps;

        public PlayerSavedData() { }

        public PlayerSavedData(string name, UInt32 lives, Dictionary<PowerUpTypes, UInt32> powerUps)
        {
            this.name = name;
            this.lives = lives;
            this.powerUps = powerUps;
        }
    }
    public static string BuildPlayerSaveFileName()
    {
        return (Application.persistentDataPath + "/player_save" + ".dat");
    }

    public static PlayerSavedData LoadPlayerSavedData()
    {
        string fileName = BuildPlayerSaveFileName();
        if (File.Exists(fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(fileName, FileMode.Open);
            PlayerSavedData savedData = new PlayerSavedData();
            savedData = (PlayerSavedData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loading from " + fileName);
            return savedData;
        }
        return null;
    }

    public static void SavePlayerData(string name, UInt32 lives, Dictionary<PowerUpTypes, UInt32> powerUps)
    {
        string fileName = BuildPlayerSaveFileName();
        if (!File.Exists(fileName))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
        BinaryFormatter BF = new BinaryFormatter();
        FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
        PlayerSavedData savedData = new PlayerSavedData(name, lives, powerUps);
        BF.Serialize(fileStream, savedData);
        Debug.Log("Saving to " + fileName);
        fileStream.Close();
    }

    #endregion
}



