using UnityEngine;
using System;

public class DiceRoller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TurnManager turnManager;
    public event Action<int> OnDiceRolled;

    private int previousRoll;
    public int PreviousRoll => previousRoll;

    /// <summary>
    /// Rolls the dice when the UI button is clicked.
    /// </summary>
    public void RollDiceFromButton()
    {
        RollDice();
    }

    /// <summary>
    /// Rolls a six-sided dice and returns the result.
    /// The result is also stored in the previousRoll 
    /// variable and an event is invoked to notify any 
    /// listeners of the new roll.
    /// </summary>
    /// <returns> integer between 1 and 6 </returns>
    public int RollDice()
    {
        if (turnManager == null)
        {
            Debug.LogError("TurnManager is not assigned!", this);
            return 0;
        }

        if (!turnManager.CanCurrentPlayerAct)
        {
            Debug.Log("Cannot roll right now. Please wait.");
            return 0;
        }

        previousRoll = UnityEngine.Random.Range(1, 7);
        Debug.Log($"Dice rolled: {previousRoll}");

        turnManager.TryMoveCurrentPlayer(previousRoll);

        OnDiceRolled?.Invoke(previousRoll);
        return previousRoll;
    }
}