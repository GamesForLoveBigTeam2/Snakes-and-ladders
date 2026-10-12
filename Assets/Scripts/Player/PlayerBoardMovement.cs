
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

    private void Awake()
    {
        if (positionTracker == null)
            positionTracker = GetComponent<PlayerPositionTracker>();
    }

    public void MovePlayer(int spacesToMove)
    {
        if (isMoving || spacesToMove <= 0)
            return;

        if (positionTracker == null || boardManager == null)
        {
            Debug.LogError("Missing movement references!");
            return;
        }

        StartCoroutine(MoveBetweenSpaces(spacesToMove));
    }

    private IEnumerator MoveBetweenSpaces(int spacesToMove)
    {
        isMoving = true;

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
                break;

            while (Vector3.Distance(
                transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            transform.position = target.position;

            // Update the existing tracking system.
            positionTracker.SetPosition(space);

            Debug.Log($"Player reached space {space}");
        }

        isMoving = false;
    }
}
