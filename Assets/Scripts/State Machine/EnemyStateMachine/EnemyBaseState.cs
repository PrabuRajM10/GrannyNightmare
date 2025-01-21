using System.Collections.Generic;
using State_Machine.EnemyStateMachine.Transitions;
using UnityEngine;

namespace State_Machine.EnemyStateMachine
{
    public abstract class EnemyBaseState : ScriptableObject
    {
        [SerializeField]protected List<Transition> transitions = new List<Transition>();
        public abstract void OnEnter(EnemyStateMachine stateMachine);
        public virtual void OnUpdate(EnemyStateMachine stateMachine)
        {
            foreach (var transition in transitions)
            {
                transition.Execute(stateMachine);
            }
        }

        public abstract void OnExit(EnemyStateMachine stateMachine);
    }
    
}