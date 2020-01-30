using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public  Level TestLevel;
}

[Serializable]
public class Level
{
    public Cake[] Cakes;
    public double[] StarRequirements;
    public int MaximumScore()
    {
        int mximumScore = 0;
        for (int i = 0; i < Cakes.Length; i++)
        {
            mximumScore +=(int)
               (((double)Cakes[i].numberOfSlices * ScoreData.NumberOfSlicesScoreNormaliser) * (double)ScoreData.ScorePointsByLevel.Awesome);
        }
        return mximumScore;
    }
}
[Serializable]
public class Cake
{
    [SerializeField]
    public GameObject cakePrefab;
    [SerializeField]
    public int numberOfSlices;
}
