using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDash : BaseState
{
    public RootDash(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    private bool enableDashCorrection;
    private float dashCorrectionTime = 0.1f;
    private float dashCorrectionCounter;
    public override void EnterState()
    {
        dashCorrectionCounter = dashCorrectionTime;
        enableDashCorrection = false;
        Debug.Log("oh no is this not being run");
        Context.impulseSource.GenerateImpulseWithForce(Context.DashShakeForce);
        Context.dashOverlay.SetActive(true);
        Context.IsDashing = true;
    }
    public override void UpdateState()
    {
        Context.rb2d.gravityScale = 0f;
        if (enableDashCorrection == false)
        {
            Context.rb2d.velocity = new Vector2(Context.rb2d.velocity.x, 0);
        }
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        if (Context.sr.flipX == true)
        {
            // Left Side Raycasts

            RaycastHit2D hitBottomLeftWall = Physics2D.Raycast(Context.BL_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
            Debug.DrawRay(Context.BL_Raycast.transform.position, Vector2.left * 0.35f, Color.red);

            RaycastHit2D hitMiddleLeftWall = Physics2D.Raycast(Context.ML_Raycast.transform.position, Vector2.left, 0.35f, Context.defineGround);
            Debug.DrawRay(Context.ML_Raycast.transform.position, Vector2.left * 0.35f, Color.red);

            if ((hitBottomLeftWall.collider != null && hitMiddleLeftWall.collider == null))
            {
                Context.rb2d.velocity = new Vector2(-Context.DashSpeed, 0);
                enableDashCorrection = true;
                Context.transform.position = new Vector2(Context.transform.position.x, Context.transform.position.y + 0.1f);
            }
            else if ((hitBottomLeftWall.collider == null && hitMiddleLeftWall.collider == null) && enableDashCorrection == true)
            {
                Debug.Log("Players' Left Dash has been successfully corrected");
                DashCorrection(-Context.DashSpeed);
            }
        }
        else
        {
            // Right Side Raycasts

            RaycastHit2D hitBottomRightWall = Physics2D.Raycast(Context.BR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);
            Debug.DrawRay(Context.BR_Raycast.transform.position, -Vector2.left * 0.35f, Color.red);

            RaycastHit2D hitMiddleRightWall = Physics2D.Raycast(Context.MR_Raycast.transform.position, -Vector2.left, 0.35f, Context.defineGround);
            Debug.DrawRay(Context.MR_Raycast.transform.position, -Vector2.left * 0.35f, Color.red);

            if ((hitBottomRightWall.collider != null && hitMiddleRightWall.collider == null))
            {
                Context.rb2d.velocity = new Vector2(Context.DashSpeed, 0);
                enableDashCorrection = true;
                Context.transform.position = new Vector2(Context.transform.position.x, Context.transform.position.y + 0.1f);
            }
            else if ((hitBottomRightWall.collider == null && hitMiddleRightWall.collider == null) && enableDashCorrection == true)
            {
                Debug.Log("Players' Right Dash has been successfully corrected");
                DashCorrection(Context.DashSpeed);
            }
        }
    }

    public override void ExitState()
    {
        Context.dashOverlay.SetActive(false);
        Context.BonusSpeedCounter += Context.BonusSpeed_Dash;
        Context.StartCountdown(Context.dashBonusSpeedTime);
        Context.dashParticle.Stop();
        Context.HasDashed = true;
        //Context.rb2d.velocity = Vector2.zero;
        Debug.Log("exit state is running");
        Context.rb2d.gravityScale = Context.originalGravityScale;
    }

    public override void CheckSwitchStates()
    {
        if (Context.JumpActivate == true)
        {
            Context.BonusHeightCounter = Context.BonusHeight_Dash;
            SwitchState(StateHandler.Jumping());
        }

        if (Context.IsDashing == false)
        {
            if (Context.onGround() == true) { SwitchState(StateHandler.Grounded()); }
            else
            {
                Context.EnableCoyoteDashJump = true;
                Context.EnableDashJumpAfterFall();
                SwitchState(StateHandler.Falling());
            }
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(StateHandler.Dashing());
    }

    public void DashCorrection(float DashPower)
    {
        Context.rb2d.velocity = new Vector2(DashPower, 0);
        dashCorrectionCounter -= Time.fixedDeltaTime;

        if (dashCorrectionCounter <= 0)
        {
            enableDashCorrection = false;
            dashCorrectionCounter = dashCorrectionTime;
        }
    }
}
