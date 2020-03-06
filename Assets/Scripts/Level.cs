using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName ="Level", menuName="Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    private string displayName;

   [SerializeField]
    private Cake[] cakes;
    [Range(0, 1)][SerializeField]
    private double[] starRequirements = { 0.6, 0.76, 0.9 };
    [SerializeField]
    private float initialTimeInSeconds = 30f;
    [Range(1,3)][SerializeField]
    private int minStarsToWin;//Ori: I thought 1 star guarantees a win


    public bool IsSucceedFirstTry { get; private set; }
    public int PlayingCount { get; set; }

    public Cake[] Cakes
    {
        get { return cakes; }
        set { cakes = value; }
    }

    public string DisplayName
    {
        get { return displayName; }
    }

    public double[] StarRequirements
    {
        get { return starRequirements; }
        set { starRequirements = value; }
    }

    public float InitialTimeInSeconds
    {
        get { return initialTimeInSeconds; }
        set { initialTimeInSeconds = value; }
    }

    public int MinStarsToWin
    {
        get { return minStarsToWin; }
        set { minStarsToWin = value; }
    }

    public void LevelSucceeded()
    {
        if(PlayingCount == 1)
        {
            IsSucceedFirstTry = true;
        }
    }

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
