using State_Machine.States.EnemyStates;
using UnityEngine;

namespace State_Machine
{
    public abstract class EnemyBaseState : ScriptableObject
    {
        public abstract void OnEnter(EnemyStateMachine stateMachine);
        public abstract void OnUpdate(EnemyStateMachine stateMachine);
        public abstract void OnExit(EnemyStateMachine stateMachine);
    }
    
}