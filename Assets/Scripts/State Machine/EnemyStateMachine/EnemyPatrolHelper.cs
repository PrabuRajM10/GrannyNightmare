using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace State_Machine.EnemyStateMachine
{
    public class EnemyPatrolHelper : MonoBehaviour
    {
        [SerializeField] private List<Transform> positionList= new List<Transform>();
        [SerializeField] private int currentIndex;
        [SerializeField] Transform targetTransform;
        [SerializeField] private float destinationBuffer = 0.5f;
        [SerializeField] private float distance;

        private void Start()
        {
            SetNextDestination();
        }

        public bool HasReachedDestination()
        {
            distance = Vector3.Distance(targetTransform.position, transform.position);
            return distance < destinationBuffer;
        }

        public void SetNextDestination()
        {
            Debug.Log("[SetNextDestination]");
            currentIndex = (currentIndex + 1) % positionList.Count;     
            targetTransform = positionList[currentIndex].transform;
            distance = Mathf.Infinity;
        }

        public Vector3 GetNextDestination()
        {
            return targetTransform.position;
        }

        public void SetPatrolPoints(PatrolPoints patrolPoints)
        {
            foreach (var point in patrolPoints.Points)
            {
                positionList.Add(point);
            }
        }
    }
}