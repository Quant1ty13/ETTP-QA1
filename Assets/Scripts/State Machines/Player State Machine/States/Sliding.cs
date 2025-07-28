using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : BaseState
{
    public Sliding(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }

    public override void EnterState()
    {
        Debug.Log("confirmed that sliding as a substate");
        // this state is just to stop moving.
    }

    public override void UpdateState()
    {
        //Context.rb2d.velocity = new Vector2(0, Context.rb2d.velocity.y);
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.enableWallSliding == false)
        {
            if (Context.Movement.x != 0) { SwitchState(StateHandler.Moving()); }
            else { SwitchState(StateHandler.Idle()); }
        }
    }

    public override void InitializeSubState()
    {
        
    }
}
