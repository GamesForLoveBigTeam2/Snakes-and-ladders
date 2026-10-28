using System;
using UnityEngine;

public class FinalSpaceDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPositionTracker positionTracker;
    [SerializeField] private BoardSpaceManager boardManager;

    public event Action OnPlayerWon;

    private bool hasWon = false;

    private void Awake()
    {
        if (positionTracker == null)
            positionTracker = GetComponent<PlayerPositionTracker>();
    }

    private void OnEnable()
    {
        if (positionTracker != null)
            positionTracker.OnPositionChanged += CheckFinalSpace;
    }

    private void OnDisable()
    {
        if (positionTracker != null)
            positionTracker.OnPositionChanged -= CheckFinalSpace;
    }

    private void CheckFinalSpace(int currentSpace)
    {
        if (hasWon || boardManager == null)
            return;

        if (currentSpace == boardManager.TotalSpaces)
        {
            hasWon = true;

            Debug.Log($"{gameObject.name} reached the final space and won!");

            OnPlayerWon?.Invoke();
        }
    }
}