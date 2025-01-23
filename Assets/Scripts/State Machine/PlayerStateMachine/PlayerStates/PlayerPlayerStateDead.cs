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
            _context.OnDead();
            _context.CharacterAnimator.SetTrigger(_context.IsDeadHash);
        }

        public override void OnExitState()
        {
        }

        public override void OnUpdateState()
        {
        }

    }
}