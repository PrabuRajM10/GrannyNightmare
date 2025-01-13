using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManagerSo;
        [SerializeField] private Bullet bullet;
        [SerializeField] private Transform bulletSpawnParent;
        private ObjectPooling<Bullet> bulletsPool;

        private void Awake()
        {
            poolManagerSo.AddPool(FactoryMethod, TurnOnBulletCallback, TurnOffBulletCallback, 100);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // bulletsPool.GetObject();
                poolManagerSo.GetPoolObject<Bullet>();
            }
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

        private Bullet FactoryMethod()
        {
            var obj = Instantiate(bullet);
            obj.Init(poolManagerSo);
            return obj;
        }
    }
}