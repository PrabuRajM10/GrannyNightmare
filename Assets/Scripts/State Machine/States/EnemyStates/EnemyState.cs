using System.Collections.Generic;
using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create EnemyState", fileName = "EnemyState", order = 0)]
    public class EnemyState : EnemyBaseState
    {
        [SerializeField]List<Activity> activities = new List<Activity>();
        [SerializeField]List<Transition> transitions = new List<Transition>();
        
        public override void OnEnter(EnemyStateMachine stateMachine)
        {
            foreach (var activity in activities)
            {
                activity.OnEnter(stateMachine);
            }
        }

        public override void OnUpdate(EnemyStateMachine stateMachine)
        {
            foreach (var activity in activities)
            {
                activity.OnExecute(stateMachine);
            }

            foreach (var transition in transitions)
            {
                transition.Execute();
            }
        }

        public override void OnExit(EnemyStateMachine stateMachine)
        {
            foreach (var activity in activities)
            {
                activity.OnExit(stateMachine);
            }
        }
    }
}
