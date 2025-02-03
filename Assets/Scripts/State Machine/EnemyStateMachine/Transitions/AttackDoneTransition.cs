using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create AttackDoneTransition", fileName = "AttackDoneTransition", order = 0)]
    public class AttackDoneTransition : Transition
    {
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if(stateMachine.IsAttackDone) stateMachine.SwitchStates(targetStates[0]);   
        }
    }
}