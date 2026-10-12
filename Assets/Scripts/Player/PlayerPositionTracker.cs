
using System;
using UnityEngine;

public class PlayerPositionTracker : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private int totalBoardSpaces = 100;

    [Header("Player Position")]
    [SerializeField] private int currentSpace = 0;

    // Allows other scripts to read the player's position.
    public int CurrentSpace => currentSpace;

    // Called whenever the player's position changes.
    public event Action<int> OnPositionChanged;

    private void Awake()
    {
        totalBoardSpaces = Mathf.Max(1, totalBoardSpaces);
        currentSpace = Mathf.Clamp(currentSpace, 0, totalBoardSpaces);
    }

    // Updates the player's current board position.
    public void SetPosition(int newSpace)
    {
        int validSpace = Mathf.Clamp(
            newSpace, 0, totalBoardSpaces
        );

        if (currentSpace == validSpace)
            return;

        currentSpace = validSpace;

        Debug.Log(
            $"{gameObject.name} moved to space {currentSpace}"
        );

        OnPositionChanged?.Invoke(currentSpace);
    }

    // Advances the player by a given number of spaces.
    public void MoveForward(int spaces)
    {
        if (spaces < 0)
        {
            Debug.LogWarning(
                "MoveForward requires a non-negative value."
            );
            return;
        }

        SetPosition(currentSpace + spaces);
    }

    // Resets the player to the starting position.
    public void ResetPosition()
    {
        SetPosition(0);
    }
}
