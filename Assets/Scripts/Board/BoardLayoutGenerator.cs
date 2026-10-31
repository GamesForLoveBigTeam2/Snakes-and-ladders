
using System.Collections.Generic;
using UnityEngine;

public class BoardLayoutGenerator : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private int rows = 10;
    [SerializeField] private int columns = 10;
    [SerializeField] private float spaceSize = 1f;

    [Header("Space Prefab")]
    [SerializeField] private GameObject spacePrefab;

    // Stores all generated spaces in numerical order.
    private readonly List<Transform> generatedSpaces =
        new List<Transform>();

    public int TotalSpaces => rows * columns;

    public List<Transform> GeneratedSpaces => generatedSpaces;

    [ContextMenu("Generate Board")]
    public void GenerateBoard()
    {
        // Check that a prefab has been assigned.
        if (spacePrefab == null)
        {
            Debug.LogError("Board space prefab is missing!");
            return;
        }

        // Validate board dimensions.
        if (rows <= 0 || columns <= 0 || spaceSize <= 0)
        {
            Debug.LogError("Invalid board dimensions!");
            return;
        }

        // Every prefab must have the BoardSpace component.
        if (spacePrefab.GetComponent<BoardSpace>() == null)
        {
            Debug.LogError(
                "The space prefab needs a BoardSpace component!"
            );
            return;
        }

        // Remove previously generated spaces.
        ClearBoard();

        int spaceNumber = 1;

        // Generate the board row by row.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Alternate direction each row.
                int boardColumn = row % 2 == 0
                    ? col
                    : columns - 1 - col;

                // Calculate the square's position.
                Vector3 position = new Vector3(
                    boardColumn * spaceSize,
                    row * spaceSize,
                    0f
                );

                // Create a new space from the prefab.
                GameObject space = Instantiate(
                    spacePrefab,
                    transform
                );

                space.name = $"Space_{spaceNumber:D3}";

                space.transform.localPosition = position;
                space.transform.localRotation = Quaternion.identity;

                // Assign the unique board-space number.
                BoardSpace boardSpace =
                    space.GetComponent<BoardSpace>();

                boardSpace.SetSpaceNumber(spaceNumber);

                // Keep the spaces in numerical order.
                generatedSpaces.Add(space.transform);

                Debug.Log(
                    $"Created board space {spaceNumber}"
                );

                spaceNumber++;
            }
        }

        Debug.Log(
            $"Board generated successfully with " +
            $"{generatedSpaces.Count} numbered spaces!"
        );
    }

    [ContextMenu("Clear Board")]
    public void ClearBoard()
    {
        // Remove previously generated board spaces.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.name.StartsWith("Space_"))
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        // Clear stored references.
        generatedSpaces.Clear();

        Debug.Log("Previous board spaces cleared.");
    }

    /// <summary>
    /// Returns the Transform of a space given its space number
    /// </summary>
    /// <param name="spaceNumber">The space number of the space </param>
    /// <returns>The Transform of the space</returns>
    public Transform GetSpaceTransform(int spaceNumber)
    {
        if (spaceNumber < 1 || spaceNumber > generatedSpaces.Count)
        {
            Debug.LogError(
                $"Invalid space number: {spaceNumber}"
            );

            return null;
        }

        int index = spaceNumber - 1;

        return generatedSpaces[index];
    }

    /// <summary>
    /// Returns the position of a space given its space number
    /// </summary>
    /// <param name="spaceNumber">The space number of the space</param>
    /// <returns>The position of the space</returns>
    public Vector3 GetSpaceWorldPosition(int spaceNumber)
    {
        Transform space = GetSpaceTransform(spaceNumber);

        if (space == null)
        {
            return Vector3.zero;
        }

        return space.position;
    }
}
