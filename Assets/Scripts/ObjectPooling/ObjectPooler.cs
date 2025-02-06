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
        [SerializeField] private PositionalAudio positionalAudio;
        [SerializeField] private Transform bulletSpawnParent;
        [SerializeField] private Transform audioSpawnParent;
        [SerializeField] private Transform enemyProjectileSpawnParent;

        private void Awake()
        {
            poolManagerSo.AddPool(BulletFactoryMethod, TurnOnBulletCallback, TurnOffBulletCallback, 100);
            poolManagerSo.AddPool(EProjectileFactory, TurnOnEnemyProjectileCallback, TurnOffEnemyProjectileCallback, 10);
            poolManagerSo.AddPool(PositionalAudioFactory, AudioTurnOnCallback, AudioTurnOffCallback, 20);
        }

        private void AudioTurnOffCallback(PositionalAudio obj)
        {
            obj.gameObject.SetActive(false);
            obj.SetParent(audioSpawnParent);
            obj.Reset();
        }

        private void AudioTurnOnCallback(PositionalAudio obj)
        {
            obj.SetParent(null);
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

        private Bullet BulletFactoryMethod()
        {
            var obj = Instantiate(bullet);
            obj.Init(poolManagerSo);
            return obj;
        }

        private PositionalAudio PositionalAudioFactory()
        {
            var obj = Instantiate(positionalAudio);
            obj.Init(poolManagerSo);        
            return obj;
        }
    }
}