using Gameplay;
using ObjectPooling;
using UnityEngine;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateRangedAttack", fileName = "EnemyStateRangedAttack", order = 0)]
    public class EnemyStateRangedAttack : EnemyBaseState
    {
        [SerializeField] private PoolManager poolManager;
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            stateMachine.StartAttack();
            stateMachine.Animator.SetBool(stateMachine.IsRangedAttackingHash, true);
            var projectile = poolManager.GetPoolObject<EnemyProjectile>();
            projectile.SetRotationAndPosition(stateMachine.ProjectileSpawnPoint);
            projectile.gameObject.SetActive(true);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            stateMachine.LookAtTarget(stateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            stateMachine.Animator.SetBool(stateMachine.IsRangedAttackingHash, false);
        }
    }
}