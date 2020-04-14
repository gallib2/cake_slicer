using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private ObstacleType type;

    public ObstacleType Type
    {
        get { return type; }
    }
}

public enum ObstacleType
{
    CHERRY,
    CANDLE
}