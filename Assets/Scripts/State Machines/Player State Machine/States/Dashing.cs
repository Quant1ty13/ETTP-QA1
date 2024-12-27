using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : BaseState
{
    public Dashing(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }
    public override void EnterState()
    {
        // any functions here will not work, because ? maybe it's being counted as a substate but the root states are not seeing it that way? nvm im a fucking dumbass
        Context.IsDashing = true;
        Context.DashActivate = false;
        Context.DashCounter = Context.DashTime;
        Debug.Log("entering the dash state");
        Dash();
    }
    public override void UpdateState()
    {
        // Set a timer here for the Dash Time
        Context.DashCounter -= Time.deltaTime;

        if (Context.DashCounter < 0)
        {
            Context.IsDashing = false;
        }
        // Constantly set variables here or disable variable settings in other states if Dashing is enabled (although that would ruin the point of a state machine but go on)
        // scratch that, maybe you could add a new rootstate called "DashAir"? to solve this problem, with this you can just enable jump through there actually.
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.IsDashing == false) // works
        {
            Context.DashActivate = false;
            Debug.Log("exiting dash state");
            if (Context.Movement.x != 0) { SwitchState(StateHandler.Moving()); }
            else { SwitchState(StateHandler.Idle()); }
        }
    }

    public override void InitializeSubState()
    {

    }

    private void Dash()
    {
        // Dash functionality here (make sure the player has an extra jump during the dash)
        if (Context.sr.flipX == false)
        {
            Context.rb2d.velocity = new Vector2(Context.DashSpeed, 0);
        }
        else
        {
            Context.rb2d.velocity = new Vector2(-Context.DashSpeed, 0);
        };
    }
}
