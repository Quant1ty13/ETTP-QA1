using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RootClimb : BaseState
{
    public RootClimb(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }
    
    // Up Parameters
    private bool sfxUPPlayed;
    private const float sfxUPCooldown = 0.175f;
    private float counterUP = sfxUPCooldown;

    // Down Parameters
    private bool sfxDOWNPlayed;
    private const float sfxDOWNCooldown = 0.1f;
    private float counterDOWN = sfxDOWNCooldown;

    public override void EnterState()
    {
        Context.player_animation.SetBool("isClimbing", true);
        Context.soundfxManager.PlayRandomSFX(Context.enterClimb, true);
        Context.ClimbingCounter = Context.ClimbingCooldown + Context.AutomaticClimbingCooldown;
        LockOn();
        Debug.Log("root climb state is now entered.");
    }
    public override void UpdateState()
    {
        if (Context.Movement.y != 0)
        {
            Context.player_animation.SetBool("isClimb_Move", true);
        }
        else if (Context.Movement.y == 0)
        {
            Context.player_animation.SetBool("isClimb_Move", false);
        }
        if (Context.onLeftWall() == false && Context.onRightWall() == false) { Context.EnableWallClimbing = false; }
        else { };
        //Context.rb2d.velocity = new Vector2(0, Context.Movement.y * Context.ClimbingSpeed);

        if (Input.GetKeyDown(KeyCode.K))
        {
            // somehow someway if this line of code is not here this entire code doesn't run???? what the fuck.
        }
        else if (!Input.GetKeyDown(KeyCode.K))
        {
            Context.rb2d.velocity = new Vector2(0, Context.Movement.y * Context.ClimbingSpeed);
        };

        if (Context.DashActivate == true)
        {
            Context.DashActivate = false;
        }

        Context.rb2d.gravityScale = 0f;
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        if (Context.Movement.y != 0)
        {
            if (Context.Movement.y < 0)
            {
                Context.ClimbingSpeed = 8.5f;
                CheckForDOWNSFX();
            }
            else if (Context.Movement.y > 0)
            {
                Context.ClimbingSpeed = 6.5f;
                CheckForUPSFX();
            }
        }
    }

    public override void ExitState()
    {
        Context.player_animation.SetBool("isClimbing", false);
        Context.player_animation.SetBool("isClimb_Move", false);

        Context.EnableWC_Cooldown = true;
        //Context.rb2d.velocity = Vector2.zero;
        Context.rb2d.gravityScale = Context.originalGravityScale;
        Context.EnableWallClimbing = false;

    }

    public override void CheckSwitchStates()
    {
        if (Context.JumpActivate == true)
        {
            Context.EnableWallClimbing = false;
            SwitchState(StateHandler.Jumping());
        }
        else { };

        if (Context.EnableWallClimbing == false && Context.JumpActivate == false)
        {
            if (Context.onGround() == true)
            {
                SwitchState(StateHandler.Grounded());
            }
            else { SwitchState(StateHandler.Falling()); }
        }
    }

    public override void InitializeSubState()
    {

    }

    private void LockOn()
    {
        if (Context.onLeftWall() == true)
        {
            Context.sr.flipX = true;
            Context.transform.position = new Vector2(Context.transform.position.x + -0.2f, Context.transform.position.y);
        }
        else if (Context.onRightWall() == true)
        {
            Context.sr.flipX = false;
            Context.transform.position = new Vector2(Context.transform.position.x + 0.2f, Context.transform.position.y);
        }
    }

    private void CheckForUPSFX()
    {
        if (sfxUPPlayed == true && counterUP <= 0)
        {
            sfxUPPlayed = false;
            counterUP = sfxUPCooldown;
            return;
        }

        if (sfxUPPlayed)
        {
            counterUP = counterUP -= Time.fixedDeltaTime;
        }
        else if (!sfxUPPlayed && !Context.JustFallen)
        {
            sfxUPPlayed = true;
            Context.soundfxManager.PlayRandomSFX(Context.climbingUp, true);
        }
    }

    private void CheckForDOWNSFX()
    {
        if (sfxDOWNPlayed == true && counterDOWN <= 0)
        {
            sfxDOWNPlayed = false;
            counterDOWN = sfxDOWNCooldown;
            return;
        }

        if (sfxDOWNPlayed)
        {
            counterDOWN = counterDOWN -= Time.fixedDeltaTime;
        }
        else if (!sfxDOWNPlayed && !Context.JustFallen)
        {
            sfxDOWNPlayed = true;
            Context.soundfxManager.PlayRandomSFX(Context.climbingUp, true);
        }
    }
}
