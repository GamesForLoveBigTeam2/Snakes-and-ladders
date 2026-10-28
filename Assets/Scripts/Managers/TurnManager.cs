using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField]
    private List<PlayerBoardMovement> players =
        new List<PlayerBoardMovement>();

    [SerializeField, Min(0)]
    private int startingPlayerIndex = 0;

    private int currentPlayerIndex;
    private bool isTurnInProgress;

    public PlayerBoardMovement CurrentPlayer
    {
        get
        {
            if (players.Count == 0)
                return null;

            if (currentPlayerIndex < 0 ||
                currentPlayerIndex >= players.Count)
                return null;

            return players[currentPlayerIndex];
        }
    }

    public int CurrentPlayerNumber => currentPlayerIndex + 1;

    public bool IsTurnInProgress => isTurnInProgress;

    public event Action<PlayerBoardMovement> OnCurrentPlayerChanged;

    public bool CanCurrentPlayerAct =>
    !isTurnInProgress &&
    CurrentPlayer != null &&
    !CurrentPlayer.IsMoving;

    private void Start()
    {
        if (players.Count == 0)
        {
            Debug.LogError("TurnManager has no players assigned.", this);
            return;
        }

        currentPlayerIndex = Mathf.Clamp(
            startingPlayerIndex, 0, players.Count - 1);

        // Listen for each player's complete movement sequence.
        foreach (PlayerBoardMovement player in players)
        {
            if (player != null)
            {
                player.OnMovementSequenceCompleted
                    += HandleMovementSequenceCompleted;
            }
        }

        AnnounceCurrentPlayer();
    }

    private void OnDestroy()
    {
        foreach (PlayerBoardMovement player in players)
        {
            if (player != null)
            {
                player.OnMovementSequenceCompleted
                    -= HandleMovementSequenceCompleted;
            }
        }
    }

    public void TryMoveCurrentPlayer(int spacesToMove)
    {
        if (isTurnInProgress)
        {
            Debug.Log("Please wait for the current turn to finish.");
            return;
        }

        if (spacesToMove <= 0)
        {
            Debug.LogWarning("The number of spaces must be positive.");
            return;
        }

        PlayerBoardMovement player = CurrentPlayer;

        if (player == null)
        {
            Debug.LogError("No valid current player is assigned.", this);
            return;
        }

        if (player.IsMoving)
        {
            Debug.LogWarning("The current player is already moving.");
            return;
        }

        isTurnInProgress = true;

        Debug.Log(
            $"{player.name}'s turn: move {spacesToMove} spaces.");

        player.MovePlayer(spacesToMove);
    }

    private void HandleMovementSequenceCompleted()
    {
        if (!isTurnInProgress)
            return;

        isTurnInProgress = false;

        Debug.Log(
            $"{CurrentPlayer.name} has finished their turn.");

        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        currentPlayerIndex =
            (currentPlayerIndex + 1) % players.Count;

        AnnounceCurrentPlayer();
    }

    private void AnnounceCurrentPlayer()
    {
        PlayerBoardMovement player = CurrentPlayer;

        if (player == null)
            return;

        Debug.Log($"It is now {player.name}'s turn.");

        OnCurrentPlayerChanged?.Invoke(player);
    }
}
