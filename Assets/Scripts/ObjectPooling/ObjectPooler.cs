using System;
using System.Collections.Generic;
using Gameplay;
using State_Machine.EnemyStateMachine;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManagerSo;
        [SerializeField] private Bullet bullet;
        [SerializeField] private EnemyProjectile enemyProjectile;
        [SerializeField] private EnemyStateMachine enemy;
        [SerializeField] private Transform bulletSpawnParent;
        [SerializeField] private Transform enemySpawnParent;
        [SerializeField] private Transform enemyProjectileSpawnParent;

        private void Awake()
        {
            poolManagerSo.AddPool(BulletFactoryMethod, TurnOnBulletCallback, TurnOffBulletCallback, 100);
            poolManagerSo.AddPool(EnemyFactoryMethod, TurnOnEnemyCallback, TurnOffEnemyCallback, 10);
            poolManagerSo.AddPool(EProjectileFactory, TurnOnEnemyProjectileCallback, TurnOffEnemyProjectileCallback, 10);
        }

        private void TurnOffEnemyProjectileCallback(EnemyProjectile obj)
        {
            obj.gameObject.SetActive(false);
            obj.SetParent(enemyProjectileSpawnParent);
            obj.Reset();
        }

        private void TurnOnEnemyProjectileCallback(EnemyProjectile obj)
        {
            obj.SetParent(null);
        }

        private EnemyProjectile EProjectileFactory()
        {
            var p = Instantiate(enemyProjectile);
            return p;
        }

        private void TurnOffBulletCallback(Bullet obj)
        {
            obj.gameObject.SetActive(false);
            obj.Reset();
            obj.SetParent(bulletSpawnParent);
        }

        private void TurnOnBulletCallback(Bullet obj)
        {
            obj.SetParent(null);
            obj.gameObject.SetActive(true);
        }
        
        private void TurnOffEnemyCallback(EnemyStateMachine obj)
        {
            obj.SetParent(enemySpawnParent);
            obj.gameObject.SetActive(false);
        }

        private void TurnOnEnemyCallback(EnemyStateMachine obj)
        {
            // obj.gameObject.SetActive(true);
        }

        private Bullet BulletFactoryMethod()
        {
            var obj = Instantiate(bullet);
            obj.Init(poolManagerSo);
            return obj;
        }
        private EnemyStateMachine EnemyFactoryMethod()
        {
            var obj = Instantiate(enemy);
            // obj.Init(poolManagerSo);
            return obj;
        }
    }
}