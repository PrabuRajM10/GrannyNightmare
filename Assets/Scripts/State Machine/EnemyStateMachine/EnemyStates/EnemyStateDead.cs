using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateDead", fileName = "EnemyStateDead", order = 0)]
    public class EnemyStateDead : EnemyBaseState
    {
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateDead] [OnEnter]");
            stateMachine.Animator.SetBool(stateMachine.IsDeadHash, true);
            stateMachine.NavAgent.enabled = false;
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
        }
    }
}