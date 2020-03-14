﻿using System.Collections;
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
  /*  [Range(1,3)][SerializeField]
    private int minStarsToWin;//Ori: I thought 1 star guarantees a win*/


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

    /*public int MinStarsToWin
    {
        get { return minStarsToWin; }
        set { minStarsToWin = value; }
    }*/

    public void LevelSucceeded()
    {
        if(PlayingCount == 1)
        {
            IsSucceedFirstTry = true;
        }
    }

    public int MaximumScore()
    {
        int maximumScore = 0;
        for (int i = 0; i < Cakes.Length; i++)
        {
            //Can be made into a lambda expression methinx
            int numberOfSlices = 0;
            if (Cakes[i].fractions.Length > 0)
            {
                for (int j = 0; j < Cakes[i].fractions.Length; j++)
                {
                    numberOfSlices += Cakes[i].fractions[j].numerator;
                }
            }
            else
            {
                numberOfSlices = Cakes[i].numberOfSlices;
            }
            maximumScore += (int)
                 (((double)numberOfSlices * ScoreData.NumberOfSlicesScoreNormaliser) *
                 (double)ScoreData.ScorePointsByLevel.Awesome);
        }
        return maximumScore;
    }

    public bool IsLegitimate()
    {
        bool isLegitimate = true;
        for (int i = 0; i < Cakes.Length; i++)
        {
            if (Cakes[i].fractions.Length > 0)
            {
                double combinedFractions = 0;
                for (int j = 0; j < Cakes[i].fractions.Length; j++)
                {
                    combinedFractions += (double)Cakes[i].fractions[j].numerator / (double)Cakes[i].fractions[j].denominator;
                }
                if(combinedFractions != 1)
                {
                    Debug.LogError("Fractions make up " + combinedFractions + " instead of 1!");
                    isLegitimate = false;
                }
            }
            else
            {
                if (Cakes[i].numberOfSlices <= 0)
                {
                    Debug.LogError("Cakes[i].numberOfSlices <= 0");
                    isLegitimate = false;
                }
            }
        }

        if (isLegitimate)
        {
            Debug.Log("Level is legitimate! (:");
        }
        else
        {
            Debug.Log("Level is NOT legitimate! );");
        }
       
        return isLegitimate;
    }
}

[Serializable]
public class Cake
{
    [SerializeField]
    public GameObject cakePrefab;
    [SerializeField][Range(2,8)]
    public byte numberOfSlices;
    public Fraction[] fractions;

}

[Serializable]
public struct Fraction
{
    [SerializeField][Range(1, 10)]
    public byte numerator;
    [SerializeField] [Range(1, 10)]
    public byte denominator;
}