using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stores ladder start and destination spaces on the board. 
/// Provides methods to check for ladders and retrieve their destinations.
/// </summary>

public class LadderManager : MonoBehaviour
{
    private Dictionary<int, int> ladders = new Dictionary<int, int>()
    {
        { 3, 22 },
        { 8, 30 },
        { 28, 56 },
        { 45, 67 },
        { 71, 92 }
    };

    public bool IsLadder(int spaceNumber)
    {
        return ladders.ContainsKey(spaceNumber);
    }

    public bool TryGetLadderDestination(int spaceNumber, out int destination)
    {
        return ladders.TryGetValue(spaceNumber, out destination);
    }

    private void Start()
    {
        int destination;

        if (TryGetLadderDestination(8, out destination))
        {
            Debug.Log($"Ladder leads to space {destination}");
        }
        else
        {
            Debug.Log("No ladder here");
        }
    }

}