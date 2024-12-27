using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDash : BaseState
{
    public RootDash(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { InitializeSubState(); IsRootState = true; }

    public override void EnterState()
    {
        Debug.Log("oh no is this not being run");
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
            Debug.Log("exiting root state through jumping");
            Context.BonusHeightCounter = Context.BonusHeight_Dash;
            SwitchState(StateHandler.Jumping());
        }

        if (Context.IsDashing == false)
        {
            Debug.Log("exiting root state through IsDashing being set to false");
            Debug.Log("IsDashing : " + Context.IsDashing);
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
