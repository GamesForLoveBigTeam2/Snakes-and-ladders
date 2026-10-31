using UnityEngine;

public class PlayerSnakeTrigger : MonoBehaviour
{
    [SerializeField] private PlayerBoardMovement playerMovement;
    [SerializeField] private PlayerPositionTracker positionTracker;
    [SerializeField] private SnakeDataManager snakeManager;

    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerBoardMovement>();

        if (positionTracker == null)
            positionTracker = GetComponent<PlayerPositionTracker>();
    }

    private void OnEnable()
    {
        if (playerMovement != null)
            playerMovement.OnMovementCompleted += CheckForSnake;
    }

    private void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.OnMovementCompleted -= CheckForSnake;
    }

    private void CheckForSnake()
    {
        if (snakeManager == null || positionTracker == null ||
            playerMovement == null)
        {
            Debug.LogWarning(
                "Missing snake manager, position tracker, " +
                "or player movement reference.", this);
            return;
        }

        int currentSpace = positionTracker.CurrentSpace;

        if (snakeManager.TryGetSnakeDestination(
            currentSpace, out int destination))
        {
            Debug.Log(
                $"Snake found! Move from {currentSpace} " +
                $"to {destination}");

            playerMovement.MoveToSpace(destination);
        }
    }
}