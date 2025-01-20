using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    public abstract class Activity : ScriptableObject
    {
        public abstract void OnEnter(EnemyStateMachine stateMachine);
        public abstract void OnExecute(EnemyStateMachine stateMachine);
        public abstract void OnExit(EnemyStateMachine stateMachine);
    }
}