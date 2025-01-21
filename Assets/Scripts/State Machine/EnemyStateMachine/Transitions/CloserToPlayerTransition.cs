using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create CloserToPlayerTransition", fileName = "CloserToPlayerTransition", order = 0)]
    public class CloserToPlayerTransition : Transition
    {
        [SerializeField] private float safeDistance = 1f;
        private PlayerStateMachine.PlayerStates.PlayerStateMachine player;
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (player == null) player = stateMachine.TargetPlayer;
            if (Vector3.Distance(stateMachine.GetPosition(), player.transform.position) < safeDistance)
            {
                stateMachine.SwitchStates(targetState); 
            }
        }
    }
}