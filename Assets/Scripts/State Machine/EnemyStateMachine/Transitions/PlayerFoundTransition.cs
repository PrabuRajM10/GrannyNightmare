using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create PlayerFoundTransition", fileName = "PlayerFoundTransition", order = 0)]
    public class PlayerFoundTransition : Transition
    {
        [SerializeField] private float overlapRadius = 5f;
        [SerializeField] private float enemyViewDistance;
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
                var hitColliders = Physics.OverlapSphere(stateMachine.GetPosition(), overlapRadius);

                foreach (var hitCollider in hitColliders)
                {
                    var playerObj = hitCollider.gameObject.GetComponent<PlayerStateMachine.PlayerStates.PlayerStateMachine>();
                    if (playerObj == null || playerObj.GetHealth() <= 0) continue;
                
                    var playerObjTransform = playerObj.transform;
                    var dirVect = (playerObjTransform.position - stateMachine.GetPosition()).normalized;
                    //Debug.Log("HandlePlayerDetection player dot result " + dotResult);

                    // if ((!(Vector3.Angle(stateMachine.transform.forward, dirVect) < viewAngle / 2))) continue;
                    var newPos = new Vector3(stateMachine.GetPosition().x, stateMachine.GetPosition().y + 0.5f, stateMachine.GetPosition().z);
         
                    if (!Physics.Raycast(newPos, dirVect, enemyViewDistance)) continue;
                    Debug.DrawLine(newPos, playerObjTransform.position, Color.yellow, 1000);
                    Debug.Log("Player got caught");
                    stateMachine.SetTargetPlayer(playerObj);
                    stateMachine.SwitchStates(targetStates[2]);
                    return;
                }
            }
            stateMachine.SwitchStates(GetTargetState(stateMachine));

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