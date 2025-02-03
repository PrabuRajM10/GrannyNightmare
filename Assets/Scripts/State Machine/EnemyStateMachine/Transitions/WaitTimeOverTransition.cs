using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create WaitTimeOverTransition", fileName = "WaitTimeOverTransition", order = 0)]
    public class WaitTimeOverTransition : Transition
    {
        [SerializeField] float waitTime = 2f;

        private float currentTime;
        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (currentTime <= waitTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                stateMachine.SwitchStates(targetStates[0]);
            }
        }
    }
}