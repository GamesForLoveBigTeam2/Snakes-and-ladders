using UnityEngine;
using UnityEngine.InputSystem;

public class FinalSpaceTester : MonoBehaviour
{
    [SerializeField] private PlayerPositionTracker positionTracker;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            positionTracker.MoveForward(1);
        }
    }
}