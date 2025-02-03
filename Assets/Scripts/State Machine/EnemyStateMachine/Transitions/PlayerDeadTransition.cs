using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create PlayerDeadTransition", fileName = "PlayerDeadTransition", order = 0)]
    public class PlayerDeadTransition : Transition
    {
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (stateMachine.TargetPlayer.GetHealth() <= 0)
            {
                stateMachine.SwitchStates(targetStates[0]);
            }
        }
    }
}