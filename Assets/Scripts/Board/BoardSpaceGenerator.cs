using UnityEngine;

public class BoardSpaceGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform boardLayout;
    [SerializeField] private BoardSpaceManager boardSpaceManager;
    [SerializeField] private GameObject spacePrefab;

    [Header("Board Settings")]
    [SerializeField, Min(1)] private int totalSpaces = 100;
    [SerializeField, Min(1)] private int columns = 10;
    [SerializeField] private float spacing = 1.2f;

    [ContextMenu("Generate Board Spaces")]
    public void GenerateBoardSpaces()
    {
        if (boardLayout == null || boardSpaceManager == null)
        {
            Debug.LogError(
                "Assign Board Layout and Board Space Manager.",
                this
            );
            return;
        }

        // Remove only previously generated spaces.
        for (int i = boardLayout.childCount - 1; i >= 0; i--)
        {
            Transform child = boardLayout.GetChild(i);

            if (child.name.StartsWith("Space_"))
                DestroyImmediate(child.gameObject);
        }

        for (int number = 1; number <= totalSpaces; number++)
        {
            int index = number - 1;
            int row = index / columns;
            int column = index % columns;

            // Reverse every other row for the zigzag pattern.
            if (row % 2 == 1)
                column = columns - 1 - column;

            Vector3 position = new Vector3(
                column * spacing,
                row * spacing,
                0f
            );

            GameObject space;

            if (spacePrefab != null)
            {
                space = Instantiate(
                    spacePrefab,
                    boardLayout
                );
            }
            else
            {
                space = new GameObject($"Space_{number:000}");

                // Create a visible placeholder tile.
                var spriteRenderer =
                    space.AddComponent<SpriteRenderer>();

                spriteRenderer.sprite =
                    CreateSquareSprite();

                spriteRenderer.sortingOrder = 0;
            }

            space.name = $"Space_{number:000}";
            space.transform.SetParent(boardLayout, false);
            space.transform.localPosition = position;
            space.transform.localRotation = Quaternion.identity;
        }

        boardSpaceManager.CollectBoardSpaces();

        Debug.Log(
            $"Generated {totalSpaces} spaces in a " +
            $"{columns}-column zigzag board.",
            this
        );
    }

    private Sprite CreateSquareSprite()
    {
        // A simple 1x1 white texture for placeholder tiles.
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, 1, 1),
            new Vector2(0.5f, 0.5f),
            1f
        );
    }
}