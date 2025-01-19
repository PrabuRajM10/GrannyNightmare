using UnityEngine;

namespace State_Machine.States.PlayerStates
{
    public class PlayerState_Run : StateMachineBase
    {
        public PlayerState_Run(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle)
        {

        }
        public override void CheckSwitchState()
        {
            Debug.Log("PlayerState_Run CheckSwitchState");

            if (!_context.CanRun) SwitchStates(stateHandle.Walk());
            else if (!_context.IsWalking) SwitchStates(stateHandle.Idle());
            else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
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
