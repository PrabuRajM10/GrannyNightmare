using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateRangedAttack", fileName = "EnemyStateRangedAttack", order = 0)]
    public class EnemyStateRangedAttack : EnemyBaseState
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.StartAttack();
            stateMachine.Animator.SetBool(stateMachine.IsRangedAttackingHash, true);
            stateMachine.EnableAttack(true);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.LookAtTarget(stateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsRangedAttackingHash, false);
            stateMachine.EnableAttack(false);
        }
    }
}