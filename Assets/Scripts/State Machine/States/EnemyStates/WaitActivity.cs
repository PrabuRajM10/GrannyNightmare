using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy Activity/Create WaitActivity", fileName = "WaitActivity", order = 0)]
    public class WaitActivity : Activity
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, true);
            stateMachine.NavAgent.isStopped = true;
        }

        public override void OnExecute(EnemyStateMachine stateMachine)
        {
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.NavAgent.isStopped = false;
            stateMachine.Animator.SetBool(stateMachine.IsIdleHash, false);
        }
    }
}