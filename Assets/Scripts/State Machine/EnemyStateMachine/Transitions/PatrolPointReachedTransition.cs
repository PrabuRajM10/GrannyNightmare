using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create PatrolPointReachedTransition", fileName = "PatrolPointReachedTransition", order = 0)]
    public class PatrolPointReachedTransition : Transition
    {
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (stateMachine.EnemyPatrolHelper.HasReachedDestination())
            {
                stateMachine.SwitchStates(targetState);
            }
        }
    }
}