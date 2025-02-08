using Gameplay;
using Helper;
using UnityEngine;
using AudioType = UnityEngine.AudioType;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateJumpAttack", fileName = "EnemyStateJumpAttack", order = 0)]
    public class EnemyStateJumpAttack : EnemyBaseState
    {
        [SerializeField] private float damageableDistance;
        [SerializeField] private int damageAmount;
        EnemyStateMachine enemyStateMachine;
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            if(enemyStateMachine == null) enemyStateMachine = stateMachine;
            enemyStateMachine.StartAttack();
            enemyStateMachine.TurnOffLocomotion(true);
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsJumpAttackingHash, true);
            enemyStateMachine.NavAgent.speed = enemyStateMachine.JumpChaseSpeed;
            // stateMachine.TurnOffLocomotion(false);
            enemyStateMachine.SubscribeDamageCalculation(DamageCalculation);
            enemyStateMachine.SubscribePlayAudio(AudioCallback);
        }
        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            base.OnUpdate(stateMachine);
            enemyStateMachine.NavAgent.SetDestination(enemyStateMachine.TargetPlayer.transform.position);
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            enemyStateMachine.Animator.SetBool(enemyStateMachine.IsJumpAttackingHash, false);
            enemyStateMachine.TurnOffLocomotion(true);
            enemyStateMachine.UnSubscribeCallbacks();
        }
        void DamageCalculation()
        {
            Debug.Log("[EnemyStateJumpAttack] [DamageCalculation] ");
            var player = Utils.GetPlayerIfInOverlap(enemyStateMachine.GetPosition(), damageableDistance);
            if(player != null)player.TakeDamage(damageAmount);
        }
        private void AudioCallback()
        {
            SoundManager.PlaySound(Gameplay.AudioType.EnemyJumpAttack , enemyStateMachine.GetPosition());
        }
        
    }
}