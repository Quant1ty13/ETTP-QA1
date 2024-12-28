using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatesHandler
{
    PlayerHandler context;

    public StatesHandler(PlayerHandler currentContext)
    {
        context = currentContext;
    }

    public BaseState Grounded() { return new Grounded(context, this); }
    public BaseState Falling() { return new Falling(context, this); }
    public BaseState Idle() { return new Idle(context, this); }
    public BaseState Jumping() { return new Jumping(context, this); }
    public BaseState Moving() { return new Moving(context, this); }
    public BaseState Dashing() { return new Dashing(context, this); }
    public BaseState RootDash() { return new RootDash(context, this); }
    public BaseState Climbing() { return new Climbing(context, this); }
    public BaseState RootClimb() { return new RootClimb(context, this); }
}
