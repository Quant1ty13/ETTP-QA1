using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : BaseState
{
    public Climbing(PlayerHandler currentContext, StatesHandler stateHandler) : base(currentContext, stateHandler) { }

    public override void EnterState()
    {
        Debug.Log("hi from the climbing state");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();

        Context.rb2d.velocity = new Vector2(0, Context.Movement.y * Context.ClimbingSpeed);

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("force applied to rigidbody!");
            //Context.rb2d.AddForce(Vector2.up * 400, ForceMode2D.Impulse);
            Context.rb2d.velocity = new Vector2(0, 64);
            Debug.Log("context.rb2d.y = " + Context.rb2d.velocity.y);
            Debug.Log("context.rb2d.x = " + Context.rb2d.velocity.x);
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.EnableWallClimbing == false) // hiding this makes it more consistent somehow????????
        {
            if (Context.Movement.x != 0) { SwitchState(StateHandler.Moving()); }
            else { SwitchState(StateHandler.Idle()); }
        }
    }

    public override void InitializeSubState()
    {

    }
}
