namespace State_Machine.States.PlayerStates
{
    public class PlayerState_Walk : StateMachineBase
    {
        public PlayerState_Walk(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}
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
