using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
/*        if (Context.JustSlideJump == true)
        {
            timeCounter += Time.deltaTime; MOVED TO PLAYERHANDLER 
        }

        if (timeCounter >= TEMPORARY_MOVE_STOP_TIME)
        {
            Context.JustSlideJump = false;
        }*/

        if (Context.externalEnableDashCooldown)
        {
            Debug.Log("State of Context.externalEnableDashCooldown" + Context.externalEnableDashCooldown);
            Context.HasDashed = false;
        }

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
/*        if (enableSlideHorizontalTimer == true && Context.JustSlideJump == true)
        {
            Debug.Log(Context.rb2d.velocity.x);
            horizontalMovement = Context.rb2d.velocity.x;
            if (horizontalMovement <= -0.1f)
            {
                horizontalMovement -= (-Context.SlideJumpPower / SLIDE_HORIZONTAL_TIME) * Time.deltaTime;
            }
            else if (horizontalMovement >= 0.1f)
            {
                horizontalMovement -= (Context.SlideJumpPower / SLIDE_HORIZONTAL_TIME) * Time.deltaTime;
            }
            Context.rb2d.velocity = new Vector2(horizontalMovement, Context.rb2d.velocity.y);
        }*/
    }

    public override void ExitState()
    {
        //Context.JustSlideJump = false;
        Context.JumpQueue = false;
        Context.JumpActivate = false;
        Context.BonusSpeedCounter += Context.BonusSpeed_Dash;
        Context.StartCountdown(Context.jumpBonusSpeedTime);
    }

    public override void CheckSwitchStates()
    {
        if (Context.rb2d.velocity.y < 0)
        {
            Context.player_animation.SetBool("isJumping", false);
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

/*        if (Context.onGround() == true)
        {
            SwitchState(StateHandler.Grounded());
        }
        else { }*/

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
        if (Context.enableWallSlideJump == true)
        {
            Debug.Log("Wall Slide Jump!");
            Context.BonusHeightCounter = 2;
            if (Context.sr.flipX == true)
            {
                // Left
                //Context.rb2d.velocity = new Vector2(Context.SlideJumpPower, Context.rb2d.velocity.y);
                Context.rb2d.AddForce(-Vector2.left * Context.SlideJumpPower, ForceMode2D.Impulse);
            }
            else
            {
                // Right
                //Context.rb2d.velocity = new Vector2(-Context.SlideJumpPower, Context.rb2d.velocity.y);
                Context.rb2d.AddForce(Vector2.left * Context.SlideJumpPower, ForceMode2D.Impulse);
            }
            Context.BonusHeightCounter = 0;
            Context.EnableSlideCooldown = true;
            Context.EnableSlideHorizontalTimer = true;
        }
        Context.EnableWallSlideJump = false;
        Context.rb2d.AddForce(Vector2.up * (Context.JumpHeight + Context.BonusHeightCounter), ForceMode2D.Impulse);
        Context.BonusHeightCounter = 0;
        Context.IsJumping = true;
    }
}
