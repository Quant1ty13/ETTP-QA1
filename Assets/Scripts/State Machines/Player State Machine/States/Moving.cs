using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Moving : BaseState
{
    public Moving(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }
    public override void EnterState()
    {
        Debug.Log("enterring moving state");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        Turn();

    }

    public override void FixedUpdateState()
    {
        Accelerate();
        Context.rb2d.velocity = new Vector2(Context.Movement.x * Context.CurrentSpeed, Context.rb2d.velocity.y);
    }

    public override void ExitState()
    {
        // 
    }

    public override void CheckSwitchStates()
    {
        // When movement.x reaches 0, switch to Idle State
        if (Context.Movement.x == 0)
        {
            SwitchState(StateHandler.Idle());
        }
        else { }

        if (Context.DashActivate == true)
        {
            Debug.Log("switching to dashing state from a substate from Moving");
            SwitchState(StateHandler.Dashing());
        }
    }

    public override void InitializeSubState()
    {

    }

    private void Turn()
    {
        if (Context.Movement.x > 0) { Context.sr.flipX = false; }
        else if (Context.Movement.x < 0) { Context.sr.flipX = true; }
    }

    private void Accelerate()
    {
        Context.CurrentSpeed += Context.AccelerationRate * Time.fixedDeltaTime;
        Context.CurrentSpeed = Mathf.Clamp(Context.CurrentSpeed, 0, Context.MaxPlayerSpeed);
    }
}
