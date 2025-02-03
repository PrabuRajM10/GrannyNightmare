using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateIdle", fileName = "EnemyStateIdle", order = 0)]
    public class EnemyStateIdle : EnemyBaseState
    {
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            // Debug.Log("[EnemyStateWait] [OnEnter]");
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, true);
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            // Debug.Log("[EnemyStateWait] [OnExit]");
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, false);
        }
    }
}
