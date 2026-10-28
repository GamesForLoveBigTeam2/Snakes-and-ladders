using System.Collections.Generic;
using UnityEngine;

public class SnakeDataManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private BoardSpaceManager boardSpaceManager;

    [Header("Snake Locations")]
    [SerializeField] private List<SnakeData> snakes =
        new List<SnakeData>();

    public IReadOnlyList<SnakeData> Snakes => snakes;

    private void Start()
    {
        ValidateSnakes();
    }

    public void ValidateSnakes()
    {
        if (boardSpaceManager == null)
        {
            Debug.LogError(
                "BoardSpaceManager is not assigned!", this);
            return;
        }

        int totalSpaces = boardSpaceManager.TotalSpaces;

        foreach (SnakeData snake in snakes)
        {
            if (snake.startSpace < 1 ||
                snake.startSpace > totalSpaces ||
                snake.destinationSpace < 1 ||
                snake.destinationSpace > totalSpaces)
            {
                Debug.LogError(
                    $"Invalid snake location: " +
                    $"{snake.startSpace} -> {snake.destinationSpace}",
                    this);
                continue;
            }

            if (snake.startSpace <= snake.destinationSpace)
            {
                Debug.LogWarning(
                    $"Snake at Space {snake.startSpace} should " +
                    $"end on a lower-numbered space.",
                    this);
            }

            Debug.Log(
                $"Snake: Space {snake.startSpace} -> " +
                $"Space {snake.destinationSpace}");
        }
    }

    public bool TryGetSnakeDestination(
    int landedSpace,
    out int destinationSpace)
{
    foreach (SnakeData snake in snakes)
    {
        if (snake.startSpace == landedSpace)
        {
            destinationSpace = snake.destinationSpace;
            return true;
        }
    }

    destinationSpace = -1;
    return false;
}
}