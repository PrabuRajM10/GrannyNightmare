using System;
using System.Collections.Generic;
using ObjectPooling;
using State_Machine.EnemyStateMachine;
using UnityEngine;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        [SerializeField] EnemyPatrolPointsHandler patrolPointsHandler;

        [SerializeField] private int enemyCount;

        private void Start()
        {
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = poolManager.GetPoolObject<EnemyStateMachine>();
                var patrolPoints = patrolPointsHandler.GetNextPatrolPoints();
                enemy.SetInitialPosition(patrolPoints.Points[0]);
                enemy.GetComponent<EnemyPatrolHelper>().SetPatrolPoints(patrolPoints);
                enemy.gameObject.SetActive(true);
            }
        }
    }
}