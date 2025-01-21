using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create NoHealthTransition", fileName = "NoHealthTransition", order = 0)]
    public class NoHealthTransition : Transition
    {
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (stateMachine.GetHealth() <= 0)
            {
                stateMachine.SwitchStates(targetState);
            }
        }
    }
}