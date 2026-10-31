using System;
using System.Collections;
using UnityEngine;

public class PlayerBoardMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPositionTracker positionTracker;
    [SerializeField] private BoardSpaceManager boardManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;

    private bool isMoving = false;

    public bool IsMoving => isMoving;

    // Fires when one movement leg finishes.
    // Snake and ladder triggers listen to this event.
    public event Action OnMovementCompleted;

    // Fires when the entire movement sequence finishes,
    // including any snake or ladder movement.
    public event Action OnMovementSequenceCompleted;

    private void Awake()
    {
        if (positionTracker == null)
            positionTracker = GetComponent<PlayerPositionTracker>();
    }

    public void MovePlayer(int spacesToMove)
    {
        if (isMoving || spacesToMove <= 0)
            return;

        if (!HasValidReferences())
            return;

        if (boardManager.TotalSpaces < 1)
        {
            Debug.LogWarning("The board has no spaces.", this);
            return;
        }

        isMoving = true;
        StartCoroutine(MoveBetweenSpaces(spacesToMove));
    }

    private IEnumerator MoveBetweenSpaces(int spacesToMove)
    {
        int currentSpace = positionTracker.CurrentSpace;

        int destination = Mathf.Min(
            currentSpace + spacesToMove,
            boardManager.TotalSpaces
        );

        for (int space = currentSpace + 1;
             space <= destination;
             space++)
        {
            Transform target = boardManager.GetBoardSpace(space);

            if (target == null)
            {
                Debug.LogError($"Board space {space} was not found.", this);
                break;
            }

            yield return MoveToPosition(target.position);

            positionTracker.SetPosition(space);

            Debug.Log($"Player reached space {space}.");
        }

        FinishMovementLeg();
    }

    public void MoveToSpace(int destinationSpace)
    {
        if (isMoving || !HasValidReferences())
            return;

        Transform target = boardManager.GetBoardSpace(destinationSpace);

        if (target == null)
        {
            Debug.LogWarning(
                $"Board space {destinationSpace} not found.", this);
            return;
        }

        // Lock movement immediately to prevent another action
        // from starting before this movement begins.
        isMoving = true;

        StartCoroutine(MoveToSpecialSpace(destinationSpace, target));
    }

    private IEnumerator MoveToSpecialSpace(
        int destinationSpace, Transform target)
    {
        yield return MoveToPosition(target.position);

        positionTracker.SetPosition(destinationSpace);

        Debug.Log($"Player moved to space {destinationSpace}.");

        FinishMovementLeg();
    }

    private IEnumerator MoveToPosition(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = destination;
    }

    private void FinishMovementLeg()
    {
        // This leg is finished.
        isMoving = false;

        // Let snake and ladder triggers check the new space.
        OnMovementCompleted?.Invoke();

        // A trigger may have started another movement leg.
        // If so, wait for that leg to finish before declaring
        // the complete sequence finished.
        if (!isMoving)
        {
            OnMovementSequenceCompleted?.Invoke();
        }
    }

    private bool HasValidReferences()
    {
        if (positionTracker == null || boardManager == null)
        {
            Debug.LogError("Missing movement references!", this);
            return false;
        }

        return true;
    }
}
