using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStatePatrol", fileName = "EnemyStatePatrol", order = 0)]
    public class EnemyStatePatrol : EnemyBaseState
    {
        private EnemyPatrolHelper enemyPatrolHelper;
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStatePatrol] [OnEnter]");
            enemyPatrolHelper = stateMachine.EnemyPatrolHelper;
            stateMachine.NavAgent.speed = stateMachine.PatrolSpeed;
            stateMachine.NavAgent.SetDestination(enemyPatrolHelper.GetNextDestination());
            stateMachine.Animator.SetBool(stateMachine.IsPatrolingHash, true);
        }
        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStatePatrol] [OnExit]");
            enemyPatrolHelper.SetNextDestination();
            stateMachine.Animator.SetBool(stateMachine.IsPatrolingHash, false);
        }
    }
}
