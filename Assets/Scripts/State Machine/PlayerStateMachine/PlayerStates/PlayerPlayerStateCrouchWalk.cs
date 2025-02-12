namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateCrouchWalk : PlayerStateMachineBase
    {
        public PlayerPlayerStateCrouchWalk(PlayerStateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle) {}
        public override void CheckSwitchState()
        {
            if(_context.CanRun) SwitchStates(stateHandle.Walk());
            else if (!_context.IsCrouching) SwitchStates(stateHandle.Idle());
            else if(!_context.IsWalking) SwitchStates(stateHandle.Crouch());
            else if (_context.IsDead) SwitchStates(stateHandle.Dead());

        }

        public override void OnEnterState()
        {
            _context.MovementSpeed = _context.CrouchWalkSpeed;
            _context.CharacterAnimator.SetBool(_context.IsCrouchWalkingHash, true);
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsCrouchWalkingHash, false);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }
    }
}
