using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : BaseState
{
    public Grounded(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        Context.player_animation.SetBool("isFalling", false);
        Context.player_animation.SetBool("isJumping", false);
        Context.player_animation.SetBool("isClimbing", false);


        Debug.Log("ground state activated");
        Context.ApexHangCounter = Context.ApexHangTime;
        Context.CoyoteTimeCounter = Context.CoyoteTime;
        Context.rb2d.gravityScale = Context.originalGravityScale;
        Context.IsJumping = false;
        Context.HasDashed = false;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();

    }

    public override void FixedUpdateState()
    {
        if (Context.onGround() == false)
        {
            Context.CoyoteTimeCounter -= Time.deltaTime;
        }
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        // Check if the Jump Buffer Timer from "Falling", is lower than the Jump Buffer counter. If so, switch states to Jumping.
        if (Context.TimeSinceJump <= Context.JumpBufferTime && Context.EnableJumpBuffer == true)
        {
            Debug.Log("switching to jumping state through jump buffer.");
            Context.TimeSinceJump = 0;
            Context.JumpActivate = true;
        }
        else if (Context.TimeSinceJump >= Context.JumpBufferTime)
        {
            Context.JumpActivate = false;
            Context.TimeSinceJump = 0;
            Context.EnableJumpBuffer = false;
        }

        if (Context.JumpActivate == true && Context.CoyoteTimeCounter >= 0) // Logic to activate jump here needs to be accompanied with Coyote Time. Or outright replace Context.JumpActive entirely.
        {
            Debug.Log("switching to jumping state through normal circumstances.");
            SwitchState(StateHandler.Jumping());
        }
        else { };

        if (Context.onGround() == false && Context.CoyoteTimeCounter <= 0)
        {
            SwitchState(StateHandler.Falling());
        }
        else { };

        if (Context.DashActivate == true)
        {
            Debug.Log("switching to dashing state from a root state");
            SwitchState(StateHandler.RootDash());
        }
        else { };

        if (Context.EnableWallClimbing == true && Context.EnableWC_Cooldown == false)
        {
            SwitchState(StateHandler.RootClimb());
        }
    }

    public override void InitializeSubState()
    {

        if (Context.Movement.x == 0 && Context.DashActivate == false)
        {
            SetSubState(StateHandler.Idle());
        }
        else if (Context.Movement.x != 0 && Context.DashActivate == false)
        {
            SetSubState(StateHandler.Moving());
        }
        else { };
        /*        else if (Context.DashActivate == true)
                {
                    Debug.Log("switching to dashing state from a root state");
                    SetSubState(StateHandler.Dashing());
                }*/
    }
}
