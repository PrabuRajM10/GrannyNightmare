using Gameplay;
using UnityEngine;
using AudioType = UnityEngine.AudioType;

namespace State_Machine.EnemyStateMachine.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy States/Create EnemyStateDead", fileName = "EnemyStateDead", order = 0)]
    public class EnemyStateDead : EnemyBaseState
    {
        EnemyStateMachine enemyStateMachine;
        public override void OnEnter(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
            Debug.Log("[EnemyStateDead] [OnEnter]");
            if(enemyStateMachine == null) enemyStateMachine = stateMachine;
            enemyStateMachine.Animator.SetBool(stateMachine.IsDeadHash, true);
            enemyStateMachine.NavAgent.enabled = false;
            enemyStateMachine.SubscribePlayAudio(AudioCallback);
            enemyStateMachine.Dead();
        }

        public override void OnExit(State_Machine.EnemyStateMachine.EnemyStateMachine stateMachine)
        {
        }
        
        private void AudioCallback()
        {
            SoundManager.PlaySound(Gameplay.AudioType.EnemyDeath , enemyStateMachine.GetPosition() , false , () =>
            {
                enemyStateMachine.UnSubscribeCallbacks();
            });
            enemyStateMachine.NavAgent.enabled = true;
        }
    }
}