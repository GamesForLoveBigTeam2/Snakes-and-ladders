using TMPro;
using UnityEngine;

/// <summary>
/// Connects dice roll results to player movement.
/// Listens for dice roll events and passes the result
/// to the player's movement system.
/// </summary>

public class DiceMovementConnector : MonoBehaviour
{
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TMP_Text diceRollResultLabel;

    private void OnEnable()
    {
        if (diceRoller != null)
            diceRoller.OnDiceRolled += HandleDiceRolled;
    }

    private void OnDisable()
    {
        if (diceRoller != null)
            diceRoller.OnDiceRolled -= HandleDiceRolled;
    }

    /// <summary>
    /// Moves the player according to the dice result.
    /// </summary>
    /// <param name="result">Number of spaces rolled.</param>
    /// 
    private void HandleDiceRolled(int result)
    {
        diceRollResultLabel.text = $"You rolled a {result}";
        
        if (turnManager == null)
        {
            Debug.LogError("TurnManager is not assigned!", this);
            return;
        }

        if (!turnManager.CanCurrentPlayerAct)
        {
            Debug.Log("Cannot roll right now. Please wait.");
            return;
        }

        turnManager.TryMoveCurrentPlayer(result);
    }
}