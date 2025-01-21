using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateChase", fileName = "EnemyStateChase", order = 0)]
    public class EnemyStateChase : EnemyBaseState
    {
        private PlayerStateMachine.PlayerStates.PlayerStateMachine player;
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateChase] [OnEnter]");
            var target = stateMachine.TargetPlayer;
            if (target != null)
            {
                player = target;
                var navAgent = stateMachine.NavAgent;
                navAgent.speed = stateMachine.ChaseSpeed;
                stateMachine.NavAgent.SetDestination(player.transform.position);
                stateMachine.Animator.SetBool(stateMachine.IsChasingHash, true);
            }
            else
            {
                Debug.LogError("[ChaseActivity] [OnEnter] target player is null");
            }
        }

        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.NavAgent.SetDestination(player.transform.position);
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateChase] [OnExit]");
            stateMachine.Animator.SetBool(stateMachine.IsChasingHash, false);
        }
    }
}