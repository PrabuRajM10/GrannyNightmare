using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateWait", fileName = "EnemyStateWait", order = 0)]
    public class EnemyStateWait : EnemyBaseState
    {
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateWait] [OnEnter]");
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, true);
            stateMachine.TurnOffLocomotion(true);
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateWait] [OnExit]");
            stateMachine.TurnOffLocomotion(false);
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, false);
        }
    }
}
