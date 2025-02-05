using State_Machine.EnemyStateMachine;
using State_Machine.PlayerStateMachine.PlayerStates;
using UnityEngine;

namespace Helper
{
    public static class Utils
    {
        public static PlayerStateMachine GetPlayerIfInOverlap(Vector3 position, float overlapRadius)
        {
            var hitColliders = Physics.OverlapSphere(position, overlapRadius);

            foreach (var hitCollider in hitColliders)
            {
                var playerObj = hitCollider.gameObject.GetComponent<PlayerStateMachine>();
                if (playerObj == null || playerObj.GetHealth() <= 0) continue;
                
                var playerObjTransform = playerObj.transform;
                var dirVect = (playerObjTransform.position - position).normalized;

                // if ((!(Vector3.Angle(stateMachine.transform.forward, dirVect) < viewAngle / 2))) continue;
                var newPos = new Vector3(position.x, position.y + 0.5f,position.z);
         
                if (!Physics.Raycast(newPos, dirVect, overlapRadius)) continue;
                // Debug.DrawLine(newPos, playerObjTransform.position, Color.yellow, 1000);
                // Debug.Log("Player got caught");
                return playerObj;
            }
            return null;
        }
        
        
    }
}
