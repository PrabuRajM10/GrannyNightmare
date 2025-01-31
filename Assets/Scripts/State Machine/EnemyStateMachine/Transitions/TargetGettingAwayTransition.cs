using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create TargetGettingAwayTransition", fileName = "TargetGettingAwayTransition", order = 0)]
    public class TargetGettingAwayTransition : Transition
    {
        [SerializeField] float minDistance;
        private PlayerStateMachine.PlayerStates.PlayerStateMachine player;

        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (player == null) player = stateMachine.TargetPlayer;
            if (Vector3.Distance(stateMachine.GetPosition(), player.transform.position) > minDistance && stateMachine.CanChasePlayer)
            {
                stateMachine.SwitchStates(targetState); 
            }
        }
    }
}