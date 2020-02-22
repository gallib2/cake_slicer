using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName ="Level", menuName="Level")]
public class Level : ScriptableObject
{
    public Cake[] Cakes;
    [Range(0, 1)]
    public double[] StarRequirements = { 0.6, 0.76, 0.9 };
    [SerializeField]
    public float initialTimeInSeconds = 30f;
    [Range(1,3)]
    public int minStarsToWin;

    public int MaximumScore()
    {
        int mximumScore = 0;
        for (int i = 0; i < Cakes.Length; i++)
        {
            mximumScore +=(int)
               (((double)Cakes[i].numberOfSlices * ScoreData.NumberOfSlicesScoreNormaliser) * 
               (double)ScoreData.ScorePointsByLevel.Awesome);
        }
        return mximumScore;
    }
}
[Serializable]
public class Cake
{
    [SerializeField]
    public GameObject cakePrefab;
    [SerializeField][Range(2,8)]
    public int numberOfSlices;
}
