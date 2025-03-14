using UnityEngine;

public class JumpProxy : MonoBehaviour
{
    private MovementStateManager movementStateManager;

    private void Awake()
    {
        movementStateManager = GetComponentInParent<MovementStateManager>();
    }

    public void Jumped() => movementStateManager.Jumped();

    public void JumpForce() => movementStateManager.JumpForce();
}