namespace State_Machine.States.PlayerStates
{
    public class PlayerState_CrouchWalk : StateMachineBase
    {
        public PlayerState_CrouchWalk(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle) {}
        public override void CheckSwitchState()
        {
            if(_context.CanRun) SwitchStates(stateHandle.Walk());
            else if (!_context.IsCrouching) SwitchStates(stateHandle.Idle());
            else if(!_context.IsWalking) SwitchStates(stateHandle.Crouch());
            else if (_context.IsKilling) ExecuteKill();

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
