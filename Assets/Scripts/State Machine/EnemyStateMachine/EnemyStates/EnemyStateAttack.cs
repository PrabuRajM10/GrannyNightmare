using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateAttack", fileName = "EnemyStateAttack", order = 0)]
    public class EnemyStateAttack : EnemyBaseState
    {
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateAttack] [OnEnter]");
            stateMachine.Animator.SetBool(stateMachine.IsAttackingHash, true);
            stateMachine.TurnOffLocomotion(true);

        }

        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.LookAtPlayer();
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateAttack] [OnExit]");
            stateMachine.TurnOffLocomotion(false);
            stateMachine.Animator.SetBool(stateMachine.IsAttackingHash, false);
        }
    }
}