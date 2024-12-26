using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BaseState
{
    private bool isRootState = false;

    private PlayerHandler context;
    private StatesHandler stateHandler;
    private BaseState currentSuperState;
    private BaseState currentSubState;

    protected bool IsRootState { set { isRootState = value; } }
    protected PlayerHandler Context { get { return context; } }
    protected StatesHandler StateHandler { get { return stateHandler; } }

    public BaseState(PlayerHandler currentContext, StatesHandler StateHandler)
    {
        context = currentContext;
        stateHandler = StateHandler;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (currentSubState != null)
        {
            currentSubState.FixedUpdateStates();
        }
    }
    protected void SwitchState(BaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (isRootState == true)
        {
            context.CurrentState = newState;
        }
        else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
    }
    protected void SetSuperState(BaseState newSuperState) 
    {
        currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubstate)
    {
        currentSubState = newSubstate;
        newSubstate.SetSuperState(this);
    }
}
