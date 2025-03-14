using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        if (movement.previousState == movement.Idle)
        {
            movement.anim.SetTrigger("IdleJump");
        }
        else if (movement.previousState == movement.Walk || movement.previousState == movement.Run)
        {
            movement.anim.SetTrigger("RunJump");
        }
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.jumped && movement.isGrounded())
        {
            movement.jumped = false;
            if (movement.hzInput == 0 && movement.vInput == 0)
            {
                movement.SwitchState(movement.Idle);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                movement.SwitchState(movement.Run);
            }
            else
            {
                movement.SwitchState(movement.Walk);
            }
        }
    }
}