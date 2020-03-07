
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;


public static class SaveAndLoadManager 
{
    public static string BuildSaveFileName()
    {
        return (Application.persistentDataPath + "/savedata" + ".dat");
    }

    public static SavedData LoadSavedData()
    {
        string fileName = BuildSaveFileName();
        if (File.Exists(fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(fileName, FileMode.Open);
            SavedData savedData = new SavedData();
            savedData = (SavedData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loading from " + fileName);
            return savedData;
        }
        else
        {
            return new SavedData(4);//Should not stay hardcoded...
        }
       // return null;
    }

    public static void SaveData(SavedData savedData)
    {
        string fileName = BuildSaveFileName();
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
        SavedData loadedSavedData = LoadSavedData();
        UInt32 oldScore = loadedSavedData.savedLevelsData[levelIndex].score;
        if (oldScore < newScore)
        {
            loadedSavedData.savedLevelsData[levelIndex].score = newScore;
            Debug.Log(oldScore + " is  smaller than " + newScore + ". Saving!");
            SaveData(loadedSavedData);
        }
        else
        {
            Debug.Log(oldScore+" is not smaller than "+ newScore+ ". No need to save");
        }
    }
}

[Serializable]
public class SavedData
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

    public SavedData() { }

    public SavedData(int numberOfLevels)
    {
        savedLevelsData = new SavedLevelData[numberOfLevels];
        for (int i = 0; i < savedLevelsData.Length; i++)
        {
            savedLevelsData[i] = new SavedLevelData(0);
        }
    }
}