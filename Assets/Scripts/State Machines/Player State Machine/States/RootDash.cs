using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDash : BaseState
{
    public RootDash(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        Debug.Log("oh no is this not being run");
        Context.dashOverlay.SetActive(true);
        Context.IsDashing = true;
    }
    public override void UpdateState()
    {
        Context.rb2d.gravityScale = 0f;
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        Context.dashOverlay.SetActive(false);
        Context.BonusSpeedCounter = Context.BonusSpeed_Dash;
        Context.StartCountdown();
        Context.dashParticle.Stop();
        Context.HasDashed = true;
        Context.rb2d.velocity = new Vector2(0, 0);
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
            if (Context.onGround() == true)
            {
                SwitchState(StateHandler.Grounded());
            }
            else { SwitchState(StateHandler.Falling()); }
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(StateHandler.Dashing());
    }
}
