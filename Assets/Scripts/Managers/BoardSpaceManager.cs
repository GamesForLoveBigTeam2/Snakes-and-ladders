
using System.Collections.Generic;
using UnityEngine;

public class BoardSpaceManager : MonoBehaviour
{
    [Header("Board Spaces")]
    [SerializeField] private List<Transform> boardSpaces =
        new List<Transform>();

    public int TotalSpaces => boardSpaces.Count;

    // Board space 1 uses list index 0.
    public Transform GetBoardSpace(int spaceNumber)
    {
        if (spaceNumber < 1 || spaceNumber > boardSpaces.Count)
        {
            Debug.LogWarning(
                $"Invalid board space: {spaceNumber}"
            );

            return null;
        }

        return boardSpaces[spaceNumber - 1];
    }
}
