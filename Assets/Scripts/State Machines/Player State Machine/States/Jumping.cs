using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseState
{
    public Jumping(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        Debug.Log("entering jumping state");
        Context.JumpActivate = false;
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
        Context.JumpQueue = false;
        Context.JumpActivate = false;
        Context.BonusSpeedCounter += Context.BonusSpeed_Dash;
        Context.StartCountdown(Context.jumpBonusSpeedTime);
    }

    public override void CheckSwitchStates()
    {
        if (Context.rb2d.velocity.y < 0)
        {
            SwitchState(StateHandler.Falling());
        }
        else { };

        if (Context.DashActivate == true && Context.HasDashed == false)
        {
            Debug.Log("switching to dashing state from a root state");
            SwitchState(StateHandler.RootDash());
        }
        else if (Context.DashActivate == true && Context.HasDashed == true)
        {
            Context.DashActivate = false;
        }
        else { };

        if (Context.EnableWallClimbing == true && Context.EnableWC_Cooldown == false)
        {
            SwitchState(StateHandler.RootClimb());
        }
        else { };

        if (Context.onGround() == true)
        {
            SwitchState(StateHandler.Grounded());
        }
        else { }

        if (Context.GameConcluded == true)
        {
            Context.GameConcluded = false;
            SwitchState(StateHandler.Grounded());
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

    public void DoJump()
    {
        if (Context.JumpQueue == true)
        {
            return;
        }
        Context.JumpQueue = true;
        Context.player_animation.SetBool("isJumping", true);
        Debug.Log("jumping!");
        Context.EnableJumpBuffer = false;
        Context.JumpActivate = true;
        Context.TimeSinceJump = 0;
        Context.soundfxManager.PlayRandomSFX(Context.jump, true);
        if (Context.enableClimbJump == true && Context.Movement.y > 0)
        {
            // Enable Boost if you're wall climbing
            Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, Context.rb2d.velocity.y);
        }
        else
        {
            // Ensures that the players' jumps are always consistent.
            Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, 0);
        }
        Context.EnableClimbJump = false;
        Context.rb2d.AddForce(Vector2.up * (Context.JumpHeight + Context.BonusHeightCounter), ForceMode2D.Impulse);
        Context.BonusHeightCounter = 0;
        Context.IsJumping = true;
    }
}
