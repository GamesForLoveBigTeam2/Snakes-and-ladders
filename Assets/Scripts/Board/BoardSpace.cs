
using TMPro;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    [Header("Space Information")]
    [SerializeField] private int spaceNumber;

    [Header("Visual Display")]
    [SerializeField] private TMP_Text numberText;

    // Other scripts can read this space's number.
    public int SpaceNumber => spaceNumber;

    public void SetSpaceNumber(int number)
    {
        if (number < 1)
        {
            Debug.LogWarning("Space number must be positive.");
            return;
        }

        spaceNumber = number;

        // Show the number on the board.
        if (numberText != null)
        {
            numberText.text = spaceNumber.ToString();
        }

        gameObject.name = $"Space_{spaceNumber:000}";
    }

    private void OnValidate()
    {
        // Keep the Inspector label synchronized.
        if (numberText != null && spaceNumber > 0)
        {
            numberText.text = spaceNumber.ToString();
        }
    }
}
