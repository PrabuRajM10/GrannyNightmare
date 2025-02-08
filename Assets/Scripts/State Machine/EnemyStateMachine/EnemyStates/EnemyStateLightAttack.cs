using Gameplay;
using Helper;
using UnityEngine;
using AudioType = Gameplay.AudioType;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateLightAttack", fileName = "EnemyStateLightAttack", order = 0)]
    public class EnemyStateLightAttack : EnemyBaseState
    {
        [SerializeField] private float damageableDistance;
        [SerializeField] private int damageAmount;
        
        EnemyStateMachine enemyStateMachine;

        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateAttack] [OnEnter]");
            if(enemyStateMachine == null) enemyStateMachine = stateMachine;
            enemyStateMachine.StartAttack();
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsLightAttackingHash, true);
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
            Debug.Log("[EnemyStateAttack] [OnExit]");
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsLightAttackingHash, false);
            enemyStateMachine.UnSubscribeCallbacks();

        }

        void DamageCalculation()
        {
            Debug.Log("[EnemyStateLightAttack] [DamageCalculation] ");

            var player = Utils.GetPlayerIfInOverlap(enemyStateMachine.GetPosition(), damageableDistance);
            if(player != null)player.TakeDamage(damageAmount);
        }

        private void AudioCallback()
        {
            SoundManager.PlaySound(AudioType.EnemyLightAttack , enemyStateMachine.GetPosition());
        }
    }
}