using Gameplay;
using ObjectPooling;
using UnityEngine;
using AudioType = Gameplay.AudioType;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateRangedAttack", fileName = "EnemyStateRangedAttack", order = 0)]
    public class EnemyStateRangedAttack : EnemyBaseState
    {
        [SerializeField] private PoolManager poolManager;
        EnemyStateMachine enemyStateMachine;
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            if(enemyStateMachine == null) enemyStateMachine = stateMachine;
            enemyStateMachine.StartAttack();
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsRangedAttackingHash, true);
            var projectile = poolManager.GetPoolObject<EnemyProjectile>();
            projectile.SetRotationAndPosition(enemyStateMachine.ProjectileSpawnPoint);
            projectile.gameObject.SetActive(true);
            enemyStateMachine.SubscribePlayAudio(AudioCallback);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            enemyStateMachine.LookAtTarget(stateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            enemyStateMachine.Animator.SetBool(stateMachine.IsRangedAttackingHash, false);
            enemyStateMachine.UnSubscribeCallbacks();
        }
        private void AudioCallback()
        {
            SoundManager.PlaySound(AudioType.EnemyRangedAttack , enemyStateMachine.GetPosition());
        }
    }
}