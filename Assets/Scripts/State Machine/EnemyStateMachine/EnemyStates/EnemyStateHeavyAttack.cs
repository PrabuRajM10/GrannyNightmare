using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateHeavyAttack", fileName = "EnemyStateHeavyAttack", order = 0)]
    public class EnemyStateHeavyAttack : EnemyBaseState
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsHeavyAttackingHash, true);
            stateMachine.TurnOffLocomotion(true);
            stateMachine.EnableAttack(true);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.LookAtTarget(stateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.TurnOffLocomotion(false);
            stateMachine.Animator.SetBool(stateMachine.IsHeavyAttackingHash, false);
            stateMachine.EnableAttack(false);
        }
    }
}