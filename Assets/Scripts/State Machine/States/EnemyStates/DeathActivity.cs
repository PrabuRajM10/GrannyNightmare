using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy Activity/Create DeathActivity", fileName = "DeathActivity", order = 0)]
    public class DeathActivity : Activity
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsDeadHash, true);
            stateMachine.NavAgent.enabled = false;
        }

        public override void OnExecute(EnemyStateMachine stateMachine)
        {
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
        }
    }
}