
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Stores ladder start and destination spaces on the board.
/// Designers can configure ladders in the Unity Inspector.
/// </summary>
public class LadderManager : MonoBehaviour
{
    [Serializable]
    public class Ladder
    {
        public int startSpace;
        public int destinationSpace;
    }

    [SerializeField]
    private List<Ladder> ladderData = new List<Ladder>();

    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    private void Awake()
    {
        foreach (Ladder ladder in ladderData)
        {
            if (ladders.ContainsKey(ladder.startSpace))
            {
                Debug.LogWarning(
                    $"Duplicate ladder starting at space {ladder.startSpace}"
                );
                continue;
            }

            ladders.Add(ladder.startSpace, ladder.destinationSpace);
        }
    }

    public bool IsLadder(int spaceNumber)
    {
        return ladders.ContainsKey(spaceNumber);
    }

    public bool TryGetLadderDestination(int spaceNumber, out int destination)
    {
        return ladders.TryGetValue(spaceNumber, out destination);
    }
}
