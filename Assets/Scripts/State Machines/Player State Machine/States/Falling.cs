using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : BaseState
{
    public Falling(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }
    public override void EnterState()
    {
        // Do the first half of Apex Hang logic
        Context.player_animation.SetBool("isJumping", false);
        Context.player_animation.SetBool("isFalling", true);
        Debug.Log("falling state now activated");
        if (Context.IsJumping == true && Context.rb2d.velocity.x <= 0)
        {
            Context.rb2d.gravityScale = 1.85f;
        }
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        // Do the second half of Apex Hang logic
        if (Context.IsJumping == true)
        {
            Context.ApexHangCounter -= Time.fixedDeltaTime;
            if (Context.ApexHangCounter < 0) { Context.rb2d.gravityScale = 5.6f; }
            else { }
        }
        else { }
    }

    public override void FixedUpdateState()
    {
        if (Context.JumpActivate == true)
        {
            Context.EnableJumpBuffer = true;
            Context.TimeSinceJump += Time.deltaTime;
        }
    }

    public override void ExitState()
    {
        // Clean-up Logic if needed
        Context.rb2d.gravityScale = Context.originalGravityScale;
    }

    public override void CheckSwitchStates()
    {
        // When onGround(True), switch to Idle State
        if (Context.onGround() == true)
        {
            Debug.Log("switching to Ground State");
            SwitchState(StateHandler.Grounded());
        }
        else if (Context.DashActivate == true && Context.HasDashed == false)
        {
            Debug.Log("switching to dashing state from a root state");
            SwitchState(StateHandler.RootDash());
        }
        else if (Context.DashActivate == true && Context.HasDashed == true)
        {
            Context.DashActivate = false;
        }
        else { };

        if (Context.EnableWallClimbing == true)
        {
            SwitchState(StateHandler.RootClimb());
        }
    }

    public override void InitializeSubState()
    {
        if (Context.Movement.x == 0)
        {
            SetSubState(StateHandler.Idle());
        }
        else if (Context.Movement.x != 0)
        {
            SetSubState(StateHandler.Moving());
        }
/*        else if (Context.DashActivate == true)
        {
            Debug.Log("switching to dashing state from a root state");
            SetSubState(StateHandler.Dashing());
        }*/
    }
}
