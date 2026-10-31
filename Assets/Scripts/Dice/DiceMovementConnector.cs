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
    [SerializeField] private Animator diceAnimator;

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
        if (!turnManager.CanCurrentPlayerAct)
        {
            Debug.Log("Cannot roll right now. Please wait.");
            return;
        }
        if (diceAnimator != null)
        {
            diceAnimator.SetInteger("DiceNum", result);
            diceAnimator.SetTrigger("Roll");
        }
        turnManager.TryMoveCurrentPlayer(result);
    }
}