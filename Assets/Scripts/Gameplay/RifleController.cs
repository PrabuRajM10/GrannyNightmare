using System;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create RifleController", fileName = "RifleController", order = 0)]
    public class RifleController : ScriptableObject
    {
        [SerializeField] private bool isReloading;
        [SerializeField] private float fireRate = 700;
        
        private float timeSinceLastShot;
        bool CanShoot() => !isReloading && timeSinceLastShot > 1f / (fireRate / 60f);
        
        public event Action OnFire;

        public event Action<bool> OnEnableWeapon;
        public void Update()
        {
            timeSinceLastShot += Time.deltaTime;
        }

        public void FireRifle()
        {
            if(!CanShoot())return;
            OnFire?.Invoke();
            timeSinceLastShot = 0f;
        }

        public void EnableWeapon(bool b)
        {
            OnEnableWeapon?.Invoke(b);
        }
    }
}