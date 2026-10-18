using UnityEngine;

public class PlayerLadderTrigger : MonoBehaviour
{
    [SerializeField] private PlayerBoardMovement playerMovement;
    [SerializeField] private PlayerPositionTracker positionTracker;
    [SerializeField] private LadderManager ladderManager;


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
            playerMovement.OnMovementCompleted += CheckForLadder;
    }

    private void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.OnMovementCompleted -= CheckForLadder;
    }

   private void CheckForLadder()
    {
        if (ladderManager == null || positionTracker == null)
        {
            Debug.LogWarning("Missing ladder or position tracker reference.");
            return;
        }

        int currentSpace = positionTracker.CurrentSpace;
        int destination;

        if (ladderManager.TryGetLadderDestination(currentSpace, out destination))
        {
            Debug.Log($"Ladder found! Move from {currentSpace} to {destination}");

            playerMovement.MoveToSpace(destination);
        }
    }

}


