namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateCrouch : PlayerStateMachineBase
    {
        public PlayerPlayerStateCrouch(PlayerStateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle) {}
        public override void CheckSwitchState()
        {
            if (_context.IsWalking) SwitchStates(stateHandle.CrouchWalk());
            else if (!_context.IsCrouching) SwitchStates(stateHandle.Idle());
        }

        public override void OnEnterState()
        {
            _context.CharacterAnimator.SetBool(_context.IsCrouchingHash, true);
            _context.CanRun = false;
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsCrouchingHash, false);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }
    }
}
