
using System.Collections.Generic;
using UnityEngine;

public class BoardNumberValidator : MonoBehaviour
{
    [SerializeField] private Transform boardLayout;
    [SerializeField] private int expectedSpaces = 100;

    [ContextMenu("Validate Board Numbers")]
    public void ValidateBoardNumbers()
    {
        if (boardLayout == null)
        {
            Debug.LogError("Board layout is missing.");
            return;
        }

        HashSet<int> numbers = new HashSet<int>();
        bool valid = true;

        foreach (Transform child in boardLayout)
        {
            BoardSpace space = child.GetComponent<BoardSpace>();

            if (space == null)
            {
                Debug.LogError(
                    $"Missing BoardSpace component: {child.name}"
                );
                valid = false;
                continue;
            }

            int number = space.SpaceNumber;

            if (number < 1 || number > expectedSpaces)
            {
                Debug.LogError($"Invalid space number: {number}");
                valid = false;
            }

            if (!numbers.Add(number))
            {
                Debug.LogError($"Duplicate space number: {number}");
                valid = false;
            }
        }

        if (numbers.Count != expectedSpaces)
        {
            Debug.LogError(
                $"Expected {expectedSpaces} unique spaces, " +
                $"but found {numbers.Count}."
            );
            valid = false;
        }

        if (valid)
        {
            Debug.Log(
                $"Board validation passed: " +
                $"{expectedSpaces} unique numbered spaces."
            );
        }
    }
}
