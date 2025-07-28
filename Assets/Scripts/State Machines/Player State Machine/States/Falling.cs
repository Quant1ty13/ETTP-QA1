using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : BaseState
{
    public Falling(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }
    public override void EnterState()
    {
        // Do the first half of Apex Hang logic
        Context.player_animation.SetBool("isFalling", true);
        Debug.Log("falling state now activated");
        if (Context.IsJumping == true && Context.rb2d.velocity.y <= 0)
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

        if (Context.rb2d.velocity.y < Context.MaxFallSpeed)
        {
            Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, Context.MaxFallSpeed);
        }
    }

    public override void FixedUpdateState()
    {
        if (Context.JumpActivate == true)
        {
            Context.EnableJumpBuffer = true;
            Context.TimeSinceJump += Time.deltaTime;
        }
        else if (Context.JumpActivate == false)
        {
            Context.EnableJumpBuffer = false;
        }
    }

    public override void ExitState()
    {
        // Clean-up Logic if needed
        Context.EnableCoyoteDashJump = false;
        Context.rb2d.gravityScale = Context.originalGravityScale;
        Context.JustFallen = true;
    }

    public override void CheckSwitchStates()
    {
        // When onGround(True), switch to Idle State
        if (Context.onGround() == true || Context.onSpring() || Context.onGround_Climb())
        {
            Debug.Log("switching to Ground State");
            Context.BonusHeightCounter = 0;
            Context.soundfxManager.PlayRandomSFX(Context.fall, true);
            SwitchState(StateHandler.Grounded());
        }
        else { };

        if (Context.enableCoyoteDashJump == true && Context.JumpActivate == true)
        {
            Context.rb2d.gravityScale = 0f;
            Context.BonusHeightCounter = Context.BonusHeight_Dash + 3.5f;
            SwitchState(StateHandler.Jumping());
        }

        if (Context.DashActivate == true && Context.HasDashed == false)
        {
            Debug.Log("switching to dashing state from a root state");
            SwitchState(StateHandler.RootDash());
        }
        else if (Context.DashActivate == true && Context.HasDashed == true)
        {
            Context.DashActivate = false;
        }

        if (Context.enableWallSliding == true && Context.enableSlideCooldown == false)
        {
            RaycastHit2D hitBottomLeftWall = Physics2D.Raycast(Context.BL_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
            RaycastHit2D hitBottomRightWall = Physics2D.Raycast(Context.BR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);

            if (hitBottomLeftWall && Context.Movement.x < -0)
            {
                SwitchState(StateHandler.RootSliding());
            }

            if (hitBottomRightWall && Context.Movement.x > 0)
            {
                SwitchState(StateHandler.RootSliding());
            }
        }

        if (Context.EnableWallClimbing == true && Context.EnableWC_Cooldown == false)
        {
            SwitchState(StateHandler.RootClimb());
        }
        else { }

        if (Context.GameConcluded == true)
        {
            Context.JumpActivate = false;
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
}
