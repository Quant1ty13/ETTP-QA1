using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
public class Moving : BaseState
{
    public Moving(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }
    private bool sfxPlayed;
    private const float sfxCooldown = 0.25f;
    private float counter = sfxCooldown;
    public override void EnterState()
    {
        Debug.Log("enterring moving state");
    }
    public override void UpdateState()
    {
        Context.player_animation.SetBool("isMoving", true);

        CheckSwitchStates();
        Turn();

        if (!Context.onGround() && (Context.onLeftWall() || Context.onRightWall()) && Context.rb2d.velocity.y < 0 && Context.EnableWallClimbing == false)
        {
            JumpCorrection();
        }



    }

    public override void FixedUpdateState()
    {
        Accelerate();
        Context.rb2d.velocity = new Vector2(Context.Movement.x * Context.CurrentSpeed, Context.rb2d.velocity.y);

        if (Context.onGround() == true && Context.JumpActivate == false)
        {
            CheckForSFX();
        }
        else if (Context.onGround() == false || Context.JumpActivate == true)
        {
            sfxPlayed = true;
            counter = sfxCooldown + 0.2f;
        }
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

        if (Context.DashActivate == true && Context.HasDashed == false)
        {
            Debug.Log("switching to dashing state from a substate from Moving");
            SwitchState(StateHandler.Dashing());
        }
        else if (Context.DashActivate == true && Context.HasDashed == true)
        {
            Context.DashActivate = false;
        }
        else { };
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
        if (Context.onGround() == false)
        {
            Context.CurrentSpeed -= Context.DecelerationRate * Time.fixedDeltaTime;
            Context.CurrentSpeed = Mathf.Clamp(Context.CurrentSpeed, Context.MaxPlayerSpeed + Context.BonusSpeedCounter / 1.25f, Context.MaxPlayerSpeed + Context.BonusSpeedCounter);

            // Footsteps
        }
        else
        {
            Context.CurrentSpeed += Context.AccelerationRate * Time.fixedDeltaTime;
            Context.CurrentSpeed = Mathf.Clamp(Context.CurrentSpeed, 0, Context.MaxPlayerSpeed + Context.BonusSpeedCounter);

            // Footsteps
        }
    }

    private void JumpCorrection()
    {
        // Nudge player to their opposite direction for a few frames, whilst respecting their current horizontal movement. WILL BE IMPLEMENTED SOON.
    }

    private void CheckForSFX()
    {
        if (sfxPlayed == true && counter <= 0)
        {
            sfxPlayed = false;
            counter = sfxCooldown;
            return;
        }

        if (sfxPlayed)
        {
            counter = counter -= Time.fixedDeltaTime;
        }
        else if (!sfxPlayed && !Context.JustFallen)
        {
            sfxPlayed = true;
            Context.soundfxManager.PlayRandomSFX(Context.footsteps, true);
        }
    }
}
