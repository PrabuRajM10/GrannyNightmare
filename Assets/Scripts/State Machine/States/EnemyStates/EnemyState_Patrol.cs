namespace State_Machine.States.EnemyStates
{
    public class EnemyState_Patrol : StateMachineBase
    {
        public EnemyState_Patrol(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}

        public override void CheckSwitchState()
        {
            if (_context.IsWaiting) SwitchStates(stateHandle.Enemy_Wait());
            if (_context.IsChasing) SwitchStates(stateHandle.Enemy_Chase());
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
