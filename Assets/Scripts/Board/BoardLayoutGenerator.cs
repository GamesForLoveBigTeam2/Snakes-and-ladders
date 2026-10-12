
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

    private readonly List<Transform> generatedSpaces =
        new List<Transform>();

    public int TotalSpaces => rows * columns;

    public List<Transform> GeneratedSpaces => generatedSpaces;

    [ContextMenu("Generate Board")]
    public void GenerateBoard()
    {
        if (spacePrefab == null)
        {
            Debug.LogError("Board space prefab is missing!");
            return;
        }

        if (rows <= 0 || columns <= 0 || spaceSize <= 0)
        {
            Debug.LogError("Invalid board dimensions.");
            return;
        }

        ClearBoard();

        int spaceNumber = 1;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int boardColumn =
                    row % 2 == 0 ? col : columns - 1 - col;

                Vector3 position = new Vector3(
                    boardColumn * spaceSize,
                    row * spaceSize,
                    0f
                );

                GameObject space = Instantiate(
                    spacePrefab,
                    transform
                );

                space.transform.localPosition = position;
                space.name = $"Space_{spaceNumber:000}";

                generatedSpaces.Add(space.transform);

                spaceNumber++;
            }
        }

        Debug.Log(
            $"Board generated with {generatedSpaces.Count} spaces."
        );
    }

    [ContextMenu("Clear Board")]
    public void ClearBoard()
    {
        // Clear only the generated board-space objects.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.name.StartsWith("Space_"))
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }
        }

        generatedSpaces.Clear();
    }
}
