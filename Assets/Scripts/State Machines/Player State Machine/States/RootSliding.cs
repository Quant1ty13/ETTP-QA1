using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSliding : BaseState
{
    public RootSliding(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    private bool HitLeftWall;
    private bool HitRightWall;
    private float slideDir;

    private bool movementUnkept;
    private const float TIME_TO_SWITCH_INPUT = 0.1f;
    private float inputCounter;

    // For Particle
    private const float TIME_FOR_EACH_PARTICLE = 0.1f;
    private float particleCounter;

    private bool EndSliding;
    public override void EnterState()
    {
        Context.rb2d.velocity = new Vector2(0, 0);
        CheckForSide();
        Context.player_animation.SetBool("isClimbing", true);
    }

    public override void UpdateState()
    {
        Context.player_animation.SetBool("isClimb_Move", true);

        if (Context.DashActivate == true)
        {
            Context.DashActivate = false;
        }

        if ((HitLeftWall == true && Context.Movement.x < -0.1f) || (HitRightWall == true && Context.Movement.x > 0.1f))
        {
            inputCounter = 0;
        }

        if ((HitLeftWall == true && Context.Movement.x > 0.1f) || (HitRightWall == true && Context.Movement.x < -0.1f) || Context.Movement.x == 0)
        {
            inputCounter += Time.deltaTime;
        }

        if (inputCounter >= TIME_TO_SWITCH_INPUT)
        {
            movementUnkept = true;
        }

        if (HitLeftWall == true)
        {
            // Left Wall Collider
            RaycastHit2D hitBottomLeftWall = Physics2D.Raycast(Context.BL_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
            if (!hitBottomLeftWall)
            {
                EndSliding = true;
            }
        }

        if (HitRightWall == true)
        {
            // Right Wall
            RaycastHit2D hitBottomRightWall = Physics2D.Raycast(Context.BR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);
            if (!hitBottomRightWall)
            {
                EndSliding = true;
            }
        }

/*        if (particleCounter <= TIME_FOR_EACH_PARTICLE)
        {
            particleCounter += Time.deltaTime;
        }

        if (particleCounter >= TIME_FOR_EACH_PARTICLE)
        {
            particleCounter = 0;
            CheckForParticle();
        }*/
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        Context.rb2d.velocity = new Vector2(0, -4f);
    }

    public override void ExitState()
    {
        Context.player_animation.SetBool("isClimbing", false);
        Context.player_animation.SetBool("isClimb_Move", false);
        Context.EnableWallSliding = false;
        HitRightWall = false;
        HitLeftWall = false;
        Context.LastSlidDirection = slideDir;
    }

    public override void CheckSwitchStates()
    {
        if (Context.JumpActivate == true)
        {
            Context.EnableWallSlideJump = true;
            Context.JustSlideJump = true;
            //Context.BonusHeightCounter = Context.BonusHeight_Dash + 2;
            SwitchState(StateHandler.Jumping());
        }

        if (movementUnkept == true || EndSliding == true)
        {
            if (Context.onGround() == true)
            {
                SwitchState(StateHandler.Grounded());
            }
            else if (Context.onGround() == false)
            {
                SwitchState(StateHandler.Falling());
            }
        }

        if (Context.onGround() == true || Context.onSpring())
        {
            Debug.Log("SWITCHING TO GROUND BITCH");
            SwitchState(StateHandler.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(StateHandler.Sliding());
    }

    private void CheckForSide()
    {
        RaycastHit2D hitBottomLeftWall = Physics2D.Raycast(Context.BL_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
        RaycastHit2D hitBottomRightWall = Physics2D.Raycast(Context.BR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);


        if (hitBottomLeftWall)
        {
            slideDir = -1f;
            HitLeftWall = true;
            Context.sr.flipX = true;
        }

        if (hitBottomRightWall)
        {
            slideDir = 1f;
            HitRightWall = true;
            Context.sr.flipX = false;
        }
    }

/*    private void CheckForParticle()
    {
        Context.slideParticle.Emit(10);
    }*/
}
