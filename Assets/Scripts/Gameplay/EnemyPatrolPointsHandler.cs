using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class EnemyPatrolPointsHandler : MonoBehaviour
    {
        [SerializeField] List<PatrolPoints> patrolPoints = new List<PatrolPoints>();

        private int currentIndex = -1;

        public PatrolPoints GetNextPatrolPoints()
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Count;
            return patrolPoints[currentIndex];
        }
    }

    [Serializable]
    public class PatrolPoints
    {
        public Transform[] Points;
    }
}