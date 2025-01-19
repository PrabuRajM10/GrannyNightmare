namespace State_Machine.States.EnemyStates
{
    public class EnemyState_Chase : StateMachineBase
    {
        public EnemyState_Chase(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle)
        {
        }

        public override void OnEnterState()
        {
            _context.CharacterAnimator.SetBool(_context.IsChasingHash,true);
            _context.SetAgentSpeed(_context.RunSpeed);
        }

        public override void OnUpdateState()
        {
            CheckSwitchState();
        }

        public override void OnExitState()
        {
            _context.CharacterAnimator.SetBool(_context.IsChasingHash,false);
        }

        public override void CheckSwitchState()
        {
            if(_context.IsAttacking) SwitchStates(stateHandle.Enemy_Attack());
        }
    }
}