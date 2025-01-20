using System.Collections.Generic;
using UnityEngine;

namespace State_Machine.States.EnemyStates
{
    public class EnemyPatrolHelper : MonoBehaviour
    {
        [SerializeField] private List<Transform> positionList= new List<Transform>();
        [SerializeField] private int currentIndex;
        [SerializeField] Transform targetTransform;
        [SerializeField] private float destinationBuffer = 0.5f;

        public bool HasReachedDestination()
        {
            return Vector3.Distance(targetTransform.position, transform.position) < destinationBuffer;
        }

        public void SetNextDestination()
        {
            currentIndex = (currentIndex + 1) % positionList.Count;     
            targetTransform = positionList[currentIndex].transform; 
        }

        public Vector3 GetNextDestination()
        {
            return targetTransform.position;
        }
    }
}