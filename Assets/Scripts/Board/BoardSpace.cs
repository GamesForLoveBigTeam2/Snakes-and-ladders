
using UnityEngine;

/// <summary>
/// Represents an individual numbered space
/// on the Snakes and Ladders board.
/// </summary>
public class BoardSpace : MonoBehaviour
{
    [Header("Board Space Settings")]
    [SerializeField] private int spaceNumber;

    // Allows other scripts to read the number.
    public int SpaceNumber => spaceNumber;

    // Sets the number assigned to this space.
    public void SetSpaceNumber(int number)
    {
        if (number < 1)
        {
            Debug.LogError(
                $"Invalid board space number: {number}"
            );
            return;
        }

        spaceNumber = number;
    }

    // Returns the current space number.
    public int GetSpaceNumber()
    {
        return spaceNumber;
    }
}
