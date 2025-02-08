namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerPlayerStateDead : PlayerStateMachineBase
    {
        public PlayerPlayerStateDead(PlayerStateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}
        public override void CheckSwitchState()
        {
        }

        public override void OnEnterState()
        {
            _context.CharacterAnimator.SetBool(_context.IsDeadHash , true);
            _context.OnDead();
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsDeadHash , false);
        }

        public override void OnUpdateState()
        {
        }

    }
}