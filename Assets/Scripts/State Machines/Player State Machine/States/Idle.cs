using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class Idle : BaseState
{
    public Idle(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }

    public override void EnterState()
    {
        Debug.Log("hi from the idle state");
    }
    public override void UpdateState()
    {
        Context.player_animation.SetBool("isMoving", false);
        CheckSwitchStates();

        Context.CurrentSpeed -= Context.DecelerationRate * Time.fixedDeltaTime;
        Context.CurrentSpeed = Mathf.Clamp(Context.CurrentSpeed, 0, Context.MaxPlayerSpeed);
        Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, Context.rb2d.velocity.y);
    }

    public override void FixedUpdateState()
    {
        Deceleration();

        if (Context.CurrentSpeed <= 0) { Context.rb2d.velocity = new Vector2(0, Context.rb2d.velocity.y); }
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.Movement.x != 0)
        {
            SwitchState(StateHandler.Moving());
        }
        else { }

        if (Context.DashActivate == true && Context.HasDashed == false)
        {
            Debug.Log("switching to dashing state from a substate from Idle");
            SwitchState(StateHandler.Dashing());
        }
        else if (Context.DashActivate == true && Context.HasDashed == true)
        {
            Context.DashActivate = false;
        }
        else { };

/*        if (Context.EnableWallClimbing == true)
        {
            SwitchState(StateHandler.Climbing());
        }*/
    }

    public override void InitializeSubState()
    {

    }

    private void Deceleration()
    {
        Context.CurrentSpeed -= Context.DecelerationRate * Time.fixedDeltaTime;
        Context.CurrentSpeed = Mathf.Clamp(Context.CurrentSpeed, 0, Context.MaxPlayerSpeed);
        Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, Context.rb2d.velocity.y);
    }
}
