using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    public abstract class Transition : ScriptableObject
    {
        [SerializeField] protected EnemyBaseState[] targetStates;
        public abstract void Execute(EnemyStateMachine stateMachine);
    }
}