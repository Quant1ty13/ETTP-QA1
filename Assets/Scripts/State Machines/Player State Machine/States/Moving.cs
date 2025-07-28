using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class Moving : BaseState
{
    public Moving(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }
    private bool jumpCorrected;
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

        if (Context.onGround())
        {
            jumpCorrected = false;
            Debug.Log("Playing Moving Particle Effect!");
            CheckForParticle();
        }

        if (Context.onGround() == false && Context.rb2d.velocity.y < 0 && Context.EnableWallClimbing == false)
        {
            //EdgeCorrection();
        }



    }

    public override void FixedUpdateState()
    {
        if (Context.justSlideJump == false || Context.enableWallSlideJump == true)
        {
            Accelerate();
            Context.rb2d.velocity = new Vector2(Context.Movement.x * Context.CurrentSpeed, Context.rb2d.velocity.y);
        }

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
        if (Context.Movement.x == 0 && Context.JustSlideJump == false)
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

/*        if (Context.enableWallSliding == true)
        {
            SwitchState(StateHandler.Sliding());
        }*/
    }

    public override void InitializeSubState()
    {

    }

    private void Turn()
    {
        if (Context.Movement.x > 0) { Context.sr.flipX = false; }
        else if (Context.Movement.x < 0) { Context.sr.flipX = true; }
        Context.moveParticle.transform.rotation = Quaternion.Euler(-90, Context.sr.flipX ? -180 : 0, 0);
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

    private void EdgeCorrection() // Note: This probably does work as intended, however this function will remain unused as it doesn't seem to fit the game.
    {
        // Left Side Raycasts
        RaycastHit2D hitBottomLeftWall = Physics2D.Raycast(Context.BL_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
        Debug.DrawRay(Context.BL_Raycast.transform.position, Vector2.left * 0.35f, Color.red);

        RaycastHit2D hitMiddleLeftWall = Physics2D.Raycast(Context.ML_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
        Debug.DrawRay(Context.ML_Raycast.transform.position, Vector2.left * 0.35f, Color.red);

        // Right Side Raycasts
        RaycastHit2D hitBottomRightWall = Physics2D.Raycast(Context.BR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);
        Debug.DrawRay(Context.BR_Raycast.transform.position, -Vector2.left * 0.35f, Color.red);

        RaycastHit2D hitMiddleRightWall = Physics2D.Raycast(Context.MR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);
        Debug.DrawRay(Context.MR_Raycast.transform.position, -Vector2.left * 0.35f, Color.red);

        if (jumpCorrected == false)
        {
            bool jumpCorrecting = false;

            if ((hitBottomLeftWall.collider != null && hitMiddleLeftWall.collider == null) ||
                (hitBottomRightWall.collider != null && hitMiddleRightWall.collider == null))
            {
                Context.transform.position = new Vector2(Context.transform.position.x, Context.transform.position.y + 0.1f);
                jumpCorrecting = true;
            }
            else if ((hitBottomLeftWall.collider == null && hitMiddleLeftWall.collider == null) ||
                (hitBottomRightWall.collider == null && hitMiddleRightWall.collider == null) && jumpCorrecting == true)
            {
                jumpCorrected = true;
                Debug.Log("Players' Jump has been successfully corrected");
            }
        }
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

    private void CheckForParticle()
    {
        Context.moveParticle.Emit(10);
    }
}
