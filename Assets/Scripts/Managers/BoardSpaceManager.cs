
using System.Collections.Generic;
using UnityEngine;

public class BoardSpaceManager : MonoBehaviour
{
    [Header("Board References")]
    [SerializeField] private Transform boardLayout;

    [SerializeField] private List<Transform> boardSpaces =
        new List<Transform>();

    public int TotalSpaces => boardSpaces.Count;

    [ContextMenu("Collect Board Spaces")]
    public void CollectBoardSpaces()
    {
        if (boardLayout == null)
        {
            Debug.LogError("Board layout is not assigned!");
            return;
        }

        boardSpaces.Clear();

        // Generated objects are named Space_001, Space_002, etc.
        // Sorting by name keeps them in numerical order.
        List<Transform> spaces = new List<Transform>();

        foreach (Transform child in boardLayout)
        {
            if (child.name.StartsWith("Space_"))
                spaces.Add(child);
        }

        spaces.Sort((a, b) =>
            string.CompareOrdinal(a.name, b.name));

        boardSpaces.AddRange(spaces);

        Debug.Log(
            $"Collected {boardSpaces.Count} board spaces."
        );
    }

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
