using UnityEngine;

namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateRun : PlayerStateMachineBase
    {
        public PlayerPlayerStateRun(PlayerStateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle)
        {

        }
        public override void CheckSwitchState()
        {
            Debug.Log("PlayerState_Run CheckSwitchState");

            if (!_context.CanRun) SwitchStates(stateHandle.Walk());
            else if (!_context.IsWalking) SwitchStates(stateHandle.Idle());
            else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
            else if (_context.IsDead) SwitchStates(stateHandle.Dead());
        }

        public override void OnEnterState()
        {
            Debug.Log("PlayerState_Run OnEnterState");

            _context.MovementSpeed = _context.RunSpeed;
            // _context.CharacterAnimator.SetBool(_context.CanRunhash, true);
        }

        public override void OnExitState()
        {
            Debug.Log("PlayerState_Run OnExitState");

            // _context.CharacterAnimator.SetBool(_context.CanRunhash, false);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }

    }
}
