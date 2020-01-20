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
    
}
[Serializable]
public class Cake
{
    [SerializeField]
    public GameObject cakePrefab;
    [SerializeField]
    public int numberOfSlices;
}
