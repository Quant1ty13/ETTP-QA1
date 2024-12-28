using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class RootClimb : BaseState
{
    public RootClimb(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        Context.ClimbingCounter = Context.ClimbingCooldown;
        LockOn();
        Debug.Log("root climb state is now entered.");
    }
    public override void UpdateState()
    {
        if (Context.onLeftWall() == false && Context.onRightWall() == false) { Context.EnableWallClimbing = false; }
        else { };
        //Context.rb2d.velocity = new Vector2(0, Context.Movement.y * Context.ClimbingSpeed);

        if (Input.GetKeyDown(KeyCode.K))
        {
            // somehow someway if this line of code is not here this entire code doesn't run???? what the fuck.
        }
        else if(!Input.GetKeyDown(KeyCode.K))
        {
            Context.rb2d.velocity = new Vector2(0, Context.Movement.y * Context.ClimbingSpeed);
        };
        Context.rb2d.gravityScale = 0f;
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        Context.EnableWC_Cooldown = true;
        Context.rb2d.velocity = new Vector2(0, 0);
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
        //SetSubState(StateHandler.Climbing());
    }

    private void LockOn()
    {
        if (Context.onLeftWall() == true)
        {
            // apply force to make the player closer to the wall.
            Context.rb2d.velocity = new Vector2(-4,1);
        }
        else if (Context.onRightWall() == true)
        {
            // apply force
            Context.rb2d.velocity = new Vector2(4, 1);
        }
    }
}
