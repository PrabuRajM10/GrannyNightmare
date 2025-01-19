namespace State_Machine.States.EnemyStates
{
    public class EnemyState_Attack : StateMachineBase
    {
        public EnemyState_Attack(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle)
        {
        }

        public override void OnEnterState()
        {
            _context.CharacterAnimator.SetBool(_context.IsAttackingHash, true);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsAttackingHash, false);
        }

        public override void CheckSwitchState()
        {
            if(_context.IsWaiting) SwitchStates(stateHandle.Enemy_Wait());
        }
    }
}