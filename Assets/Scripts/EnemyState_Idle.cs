using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : StateMachineBase
{
    public EnemyState_Idle(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}

    public override void CheckSwitchState()
    {
        if (_context.IsWalking) SwitchStates(stateHandle.Enemy_Walk());
    }

    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
        CheckSwitchState();
    }
}
