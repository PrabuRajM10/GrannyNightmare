using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateJumpAttack", fileName = "EnemyStateJumpAttack", order = 0)]
    public class EnemyStateJumpAttack : EnemyBaseState
    {
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.StartAttack();
            stateMachine.TurnOffLocomotion(true);
            stateMachine.Animator.SetBool(stateMachine.IsJumpAttackingHash, true);
            stateMachine.NavAgent.speed = stateMachine.JumpChaseSpeed;
            // stateMachine.TurnOffLocomotion(false);
            stateMachine.EnableAttack(true);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.NavAgent.SetDestination(stateMachine.TargetPlayer.transform.position);
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsJumpAttackingHash, false);
            stateMachine.EnableAttack(false);
            stateMachine.TurnOffLocomotion(true);
        }
    }
}