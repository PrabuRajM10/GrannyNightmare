using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateAttack", fileName = "EnemyStateAttack", order = 0)]
    public class EnemyStateLightAttack : EnemyBaseState
    {
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateAttack] [OnEnter]");
            stateMachine.Animator.SetBool(stateMachine.IsLightAttackingHash, true);
            stateMachine.TurnOffLocomotion(true);
            stateMachine.EnableAttack(true);
        }

        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.LookAtTarget(stateMachine.TargetPlayer.transform);
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateAttack] [OnExit]");
            stateMachine.TurnOffLocomotion(false);
            stateMachine.Animator.SetBool(stateMachine.IsLightAttackingHash, false);
            stateMachine.EnableAttack(false);
        }
    }
}