using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy Activity/Create ChaseActivity", fileName = "ChaseActivity", order = 0)]
    public class ChaseActivity : Activity
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            var target = stateMachine.TargetPlayer;
            if (target != null)
            {
                var navAgent = stateMachine.NavAgent;
                navAgent.speed = stateMachine.ChaseSpeed;
                stateMachine.NavAgent.SetDestination(target.transform.position);
                stateMachine.Animator.SetBool(stateMachine.IsChasingHash, true);
            }
            else
            {
                Debug.LogError("[ChaseActivity] [OnEnter] target player is null");
            }
        }

        public override void OnExecute(EnemyStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsChasingHash, false);
        }
    }
}