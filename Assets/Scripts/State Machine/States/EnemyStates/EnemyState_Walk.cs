namespace State_Machine.States.EnemyStates
{
    public class EnemyState_Walk : StateMachineBase
    {
        public EnemyState_Walk(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}

        public override void CheckSwitchState()
        {
            if (!_context.IsWalking) SwitchStates(stateHandle.Enemy_Idle());
        }

        public override void OnEnterState()
        {
            _context.CharacterAnimator.SetBool(_context.IsWalkingHash, true);
            _context.SetAgentSpeed(_context.WalkSpeed);
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsWalkingHash, false);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }
    }
}
