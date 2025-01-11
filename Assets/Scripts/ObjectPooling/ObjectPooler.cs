using System;
using Gameplay;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private Bullet bullet;
        private ObjectPooling<Bullet> bulletsPool;

        private void Awake()
        {
            bulletsPool = new ObjectPooling<Bullet>(FactoryMethod, TurnOnCallback, TurnOffCallback, 10);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bulletsPool.GetObject();
            }
        }

        private void TurnOffCallback(Bullet obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void TurnOnCallback(Bullet obj)
        {
            obj.gameObject.SetActive(true);
        }

        private Bullet FactoryMethod()
        {
            return Instantiate(bullet);
        }

        private void Start()
        {
            
        }
    }
}