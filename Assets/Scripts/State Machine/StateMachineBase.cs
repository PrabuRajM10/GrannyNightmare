using UnityEngine;

public abstract class StateMachineBase
{
    protected StateMachine _context;
    protected StateMachineHandle stateHandle;

    public StateMachineBase(StateMachine context, StateMachineHandle playerStateHandle)
    {
        _context = context;  
        stateHandle= playerStateHandle;  
    }
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
    public abstract void CheckSwitchState();

    protected void SwitchStates(StateMachineBase newState)
    {
        Debug.Log(" SwitchStates " + newState.ToString());
        OnExitState();
        newState.OnEnterState();
        _context.CurrentState = newState;
    }

}
