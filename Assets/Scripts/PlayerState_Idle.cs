using UnityEngine;

public class PlayerState_Idle : StateMachineBase
{

    public PlayerState_Idle(StateMachine context , StateMachineHandle playerStateHandle) : base(context , playerStateHandle){}
    public override void CheckSwitchState()
    {
        if (_context.IsWalking) SwitchStates(stateHandle.Walk());
        else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
        else if (_context.IsKilling) ExecuteKill();
    }

    public override void OnEnterState()
    {
        _context.MovementSpeed = 0;
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {

        CheckSwitchState();
    }
}
