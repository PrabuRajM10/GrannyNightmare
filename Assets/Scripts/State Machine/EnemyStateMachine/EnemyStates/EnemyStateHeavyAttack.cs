using Helper;
using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateHeavyAttack", fileName = "EnemyStateHeavyAttack", order = 0)]
    public class EnemyStateHeavyAttack : EnemyBaseState
    {
        [SerializeField] private float damageableDistance;
        [SerializeField] private int damageAmount;
        EnemyStateMachine enemyStateMachine;

        public override void OnEnter(EnemyStateMachine stateMachine)
        {           
            if(enemyStateMachine == null) enemyStateMachine = stateMachine;
            enemyStateMachine.StartAttack();
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsHeavyAttackingHash, true);
            enemyStateMachine.SubscribeDamageCalculation(DamageCalculation);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            enemyStateMachine.LookAtTarget(enemyStateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsHeavyAttackingHash, false);
            enemyStateMachine.UnSubscribeDamageCalculation();
        }
        void DamageCalculation()
        {
            Debug.Log("[EnemyStateHeavyAttack] [DamageCalculation] ");
            Debug.Log("[EnemyStateLightAttack] [DamageCalculation] ");

            var player = Utils.GetPlayerIfInOverlap(enemyStateMachine.GetPosition(), damageableDistance);
            if(player != null)player.TakeDamage(damageAmount);
        }
    }
}