using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseState
{
    public Jumping(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        DoJump();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        Context.JumpActivate = false;
    }

    public override void CheckSwitchStates()
    {
        if (Context.rb2d.velocity.y < 0)
        {
            SwitchState(StateHandler.Falling());
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
        else if (Context.DashActivate == true)
        {
            Debug.Log("switching to dashing state from a root state");
            SetSubState(StateHandler.Dashing());
        }
    }

    public void DoJump()
    {
        Context.player_animation.SetBool("isJumping", true);

        Debug.Log("jumping!");
        Context.EnableJumpBuffer = false;
        Context.JumpActivate = true;
        Context.TimeSinceJump = 0;
        Context.soundfxManager.PlaySFX(Context.jump, true);
        Context.rb2d.AddForce(Vector2.up * Context.JumpHeight, ForceMode2D.Impulse);
        Context.CanJump = false;
        Context.IsJumping = true;
    }
}
