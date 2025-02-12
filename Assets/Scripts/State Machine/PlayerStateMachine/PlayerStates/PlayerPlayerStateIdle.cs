namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateIdle : PlayerStateMachineBase
    {

        public PlayerPlayerStateIdle(PlayerStateMachine context , StateMachineHandle playerStateHandle) : base(context , playerStateHandle){}
        public override void CheckSwitchState()
        {
            if (_context.IsWalking) SwitchStates(stateHandle.Walk());
            else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
            else if (_context.IsDead) SwitchStates(stateHandle.Dead());
        }

        public override void OnEnterState()
        {
            _context.MovementSpeed = 0;
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
