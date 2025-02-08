using Gameplay;
using Helper;
using UnityEngine;
using AudioType = UnityEngine.AudioType;

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
            enemyStateMachine.SubscribePlayAudio(AudioCallback);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            enemyStateMachine.LookAtTarget(enemyStateMachine.TargetPlayer.transform);
        }
        public override void OnExit(EnemyStateMachine stateMachine)
        {
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsHeavyAttackingHash, false);
            enemyStateMachine.UnSubscribeCallbacks();
        }
        void DamageCalculation()
        {
            var player = Utils.GetPlayerIfInOverlap(enemyStateMachine.GetPosition(), damageableDistance);
            if(player != null)player.TakeDamage(damageAmount);
        }
        
        private void AudioCallback()
        {
            SoundManager.PlaySound(Gameplay.AudioType.EnemyHeavyAttack , enemyStateMachine.GetPosition());
        }
    }
}