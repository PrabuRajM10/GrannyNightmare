using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Walk : StateMachineBase
{
    public EnemyState_Walk(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}

    public override void CheckSwitchState()
    {
        Debug.Log("EnemyState_Walk CheckSwitchState");
        if (!_context.IsWalking) SwitchStates(stateHandle.Enemy_Idle());
    }

    public override void OnEnterState()
    {
        Debug.Log("EnemyState_Walk OnEnterState");
        _context.CharacterAnimator.SetBool(_context.IsWalkinghash, true);
    }

    public override void OnExitState()
    {
        Debug.Log("EnemyState_Walk OnExitState");
        _context.CharacterAnimator.SetBool(_context.IsWalkinghash, false);
    }

    public override void OnUpdateState()
    {
        Debug.Log("EnemyState_Walk OnUpdateState");
        CheckSwitchState();
    }

   
}
