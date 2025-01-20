using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy Activity/Create PatrolActivity", fileName = "PatrolActivity", order = 0)]
    public class PatrolActivity : Activity
    {
        private EnemyPatrolHelper enemyPatrolHelper;
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            enemyPatrolHelper = stateMachine.EnemyPatrolHelper;
            stateMachine.NavAgent.speed = stateMachine.PatrolSpeed;
            stateMachine.NavAgent.SetDestination(enemyPatrolHelper.GetNextDestination());
            stateMachine.Animator.SetBool(stateMachine.IsChasingHash, true);

        }

        public override void OnExecute(EnemyStateMachine stateMachine)
        {
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            enemyPatrolHelper.SetNextDestination();
            stateMachine.Animator.SetBool(stateMachine.IsChasingHash, false);
        }
    }
}