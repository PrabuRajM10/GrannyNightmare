using UnityEngine;

namespace State_Machine.EnemyStateMachine.Transitions
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create Transition/Create PlayerFoundTransition", fileName = "PlayerFoundTransition", order = 0)]
    public class PlayerFoundTransition : Transition
    {
        [SerializeField] private float overlapRadius = 5f;
        [SerializeField] private float enemyViewDistance;
        private float viewAngle;

        public override void Execute(EnemyStateMachine stateMachine)
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
                stateMachine.SwitchStates(targetState);
            }
        }
    }
}