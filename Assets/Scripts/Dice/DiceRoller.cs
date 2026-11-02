using UnityEngine;
using System;

public class DiceRoller : MonoBehaviour
{
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
        previousRoll = UnityEngine.Random.Range(1, 7);

        AudioManager.Instance?.PlayDiceRoll(previousRoll);
        AudioManager.Instance?.PlaySound(SoundType.Bubble);

        OnDiceRolled?.Invoke(previousRoll);
        Debug.Log($"Dice rolled: {previousRoll}");

        return previousRoll;
    }
}