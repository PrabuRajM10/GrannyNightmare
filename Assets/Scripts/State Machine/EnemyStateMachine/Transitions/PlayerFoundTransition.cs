using Helper;
using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create PlayerFoundTransition", fileName = "PlayerFoundTransition", order = 0)]
    public class PlayerFoundTransition : Transition
    {
        [SerializeField] private float overlapRadius = 5f;
        [SerializeField] private float attackDistance;
        [SerializeField] private float chaseDistance;
        [SerializeField] private float jumpAttackDistance;
        [SerializeField] private float rangedAttackDistance;
        private float viewAngle;
        // [SerializeField] private float distance;

        private int[] actionIndex = new[] { 2, 3, 4 };

        public override void Execute(EnemyStateMachine stateMachine)
        {
            if (stateMachine.TargetPlayer == null)
            {
                var player = Utils.GetPlayerIfInOverlap(stateMachine.GetPosition(), overlapRadius);
                stateMachine.SetTargetPlayer(player);
                stateMachine.SwitchStates(targetStates[2]);
            }
            if(!stateMachine.TargetPlayer.IsDead)stateMachine.SwitchStates(GetTargetState(stateMachine));

        }

        EnemyBaseState GetTargetState(EnemyStateMachine stateMachine)
        {
            var distance = Vector3.Distance(stateMachine.TargetPlayer.transform.position, stateMachine.GetPosition());
            
            // [0] - light 
            // [1] - heavy
            // [2] - chase
            // [3] - jump
            // [4] - ranged
            if (distance <= attackDistance)
            {
                return targetStates[Random.Range(0,2)];
            }
            if (distance > attackDistance && distance < jumpAttackDistance)
            {
                return targetStates[2];
            }
            if (distance > chaseDistance && distance < rangedAttackDistance)
            {
                return targetStates[Random.Range(3,4)];
            }

            if (!(distance >= rangedAttackDistance)) return targetStates[1];
            var rand = actionIndex[Random.Range(0, actionIndex.Length)];
            Debug.Log("[GetTargetState] rand " +rand);
            return targetStates[rand]; 
        }
    }
}