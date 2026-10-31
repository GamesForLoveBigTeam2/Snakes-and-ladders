using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private TextMeshProUGUI playerTurn;
    [SerializeField]
    private List<PlayerBoardMovement> players =
        new List<PlayerBoardMovement>();

    [SerializeField, Min(0)]
    private int startingPlayerIndex = 0;

    private int currentPlayerIndex;
    private bool isTurnInProgress;

    private bool gameOver = false;

    public bool GameOver => gameOver;

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
    !gameOver &&
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
            if (player == null)
                continue;

            player.OnMovementSequenceCompleted +=
                HandleMovementSequenceCompleted;

            FinalSpaceDetector detector =
                player.GetComponent<FinalSpaceDetector>();

            if (detector != null)
                detector.OnPlayerWon += HandlePlayerWon;
            else
                Debug.LogWarning(
                    $"No FinalSpaceDetector found on {player.name}.",
                    player
                );
        }

        AnnounceCurrentPlayer();
    }

    private void OnDestroy()
    {
        foreach (PlayerBoardMovement player in players)
        {
            if (player == null)
                continue;

            player.OnMovementSequenceCompleted -=
                HandleMovementSequenceCompleted;

            FinalSpaceDetector detector =
                player.GetComponent<FinalSpaceDetector>();

            if (detector != null)
                detector.OnPlayerWon -= HandlePlayerWon;
        }
    }

    public void TryMoveCurrentPlayer(int spacesToMove)
    {
        if (gameOver)
        {
            Debug.Log("The game is over. No more moves are allowed.");
            return;
        }
        
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
        if (gameOver || !isTurnInProgress)
            return;

        isTurnInProgress = false;

        Debug.Log($"{CurrentPlayer.name} has finished their turn.");

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

        playerTurn.text = $"It's {player.name}'s turn.";

        OnCurrentPlayerChanged?.Invoke(player);
        
        // Auto Play for CPU Players
        int humanCount = TotalPlayerManager.HumanPlayerCount;
        if (humanCount > 0 && CurrentPlayerNumber > humanCount) { AutoPlay(); }
    }

    private void HandlePlayerWon()
    {
        if (gameOver)
            return;

        gameOver = true;

        Debug.Log("GAME OVER! A player has won.");

        // Prevent any further turn progression.
        isTurnInProgress = false;
    }

    private void AutoPlay()
{
    if (!CanCurrentPlayerAct) { return; }

    // Example: automatically roll a six-sided die.
    int spacesToMove = UnityEngine.Random.Range(1, 7);

    TryMoveCurrentPlayer(spacesToMove);
}
}
