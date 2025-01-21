namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateWalk : PlayerStateMachineBase
    {
        public PlayerPlayerStateWalk(PlayerStateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}
        public override void CheckSwitchState()
        {
            if (_context.CanRun) SwitchStates(stateHandle.Run());
            else if (!_context.IsWalking) SwitchStates(stateHandle.Idle());
            else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
        }

        public override void OnEnterState()
        {
            _context.MovementSpeed = _context.WalkSpeed;
        }

        public override void OnExitState()
        {
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }

    }
}
