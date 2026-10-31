using System;
using UnityEngine;

[Serializable]
public class SnakeData
{
    [Header("Snake Locations")]
    [Range(1, 100)]
    public int startSpace = 24;

    [Range(1,100)]
    public int destinationSpace = 7;
}