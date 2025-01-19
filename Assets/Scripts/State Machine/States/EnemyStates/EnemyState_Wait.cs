using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    public class EnemyState_Wait : StateMachineBase
    {
        float currentTime = 0f;
        float restTime = 0f;
        public EnemyState_Wait(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}

        public override void CheckSwitchState()
        {
            if (_context.IsWalking) SwitchStates(stateHandle.Enemy_Patrol());
            if (_context.IsChasing) SwitchStates(stateHandle.Enemy_Chase());
        }

        public override void OnEnterState()
        {
            // _context.CharacterAnimator.SetBool(_context.IsWaitingHash , false);
            restTime = Random.Range(3, 7);
        }

        public override void OnExitState()
        {
            // _context.CharacterAnimator.SetBool(_context.IsWaitingHash , false);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
            // if (currentTime < restTime)
            // {
            //     currentTime += Time.deltaTime;
            // }
            // else
            // {
            //     currentTime = 0f;
            //     SwitchStates(stateHandle.Enemy_Patrol());
            // }
        }
    }
}
