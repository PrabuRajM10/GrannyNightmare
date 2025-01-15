namespace State_Machine.States.PlayerStates
{
    public class PlayerState_Kill : StateMachineBase
    {
        public PlayerState_Kill(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle)
        {

        }
        public override void CheckSwitchState()
        {

        }

        public override void OnEnterState()
        {
            HandleKill();
        }

        public override void OnExitState()
        {

        }

        public override void OnUpdateState()
        {
        }

        void HandleKill()
        {
            //HandleKillHintUI(false);
            //var newPos = new Vector3(_context.PlayerTransform.position.x, _context.PlayerTransform.position.y, _context.LockedEnemy.position.z - 1.5f);
            //_context.PlayerTransform.position = newPos;
            //_context.PlayerTransform.LookAt(_context.LockedEnemy.position);

            //_context.CharacterController.enabled = false;
            //_context.PlayerAnimator.SetTrigger(_context.IsKillingHash);
            //var enemyAnimator = _context.LockedEnemy.GetComponent<Animator>();
            //if (enemyAnimator != null)
            //{
            //    enemyAnimator.SetTrigger("IsDead");
            //}
            //_context.LockedEnemy.GetComponent<Collider>().enabled = false;
            //_context.LockedEnemy = null;

            //_context.CharacterController.enabled = true;

        }
    }
}
