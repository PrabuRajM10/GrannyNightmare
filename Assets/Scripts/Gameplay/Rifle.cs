using System;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Rifle : MonoBehaviour
    {
        [SerializeField] PoolManager poolManagerSo;
        [SerializeField] RifleController rifleControllerSo;
        [SerializeField] private Transform muzzle;
        [SerializeField] private GameObject mesh;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask ignoreLayer;

        private Vector3 lookTarget;

        private void OnValidate()
        {
            if(mainCamera == null)mainCamera = Camera.main; 
        }

        private void OnDisable()
        {
            rifleControllerSo.OnFire -= Fire;
            rifleControllerSo.OnEnableWeapon -= OnEnableWeapon;
        }

        private void OnEnable()
        {
            rifleControllerSo.OnFire += Fire;
            rifleControllerSo.OnEnableWeapon += OnEnableWeapon;
        }

        private void Update()
        {
            rifleControllerSo.Update();
        }

        private void Fire()
        {
            var screenCentre = new Vector3(Screen.width/2, Screen.height/2 , 0);
            var ray = mainCamera.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity , ~ignoreLayer))
            {
                var hitPoint = hit.point;

                var bulletDir = (hitPoint - muzzle.position) .normalized;
                
                Debug.DrawLine(muzzle.position, hitPoint, Color.red , 1000);
                Debug.Log("[Fire] hit name " + hit.collider.name);
                
                var bullet = poolManagerSo.GetPoolObject<Bullet>();
                bullet.SetRotationAndPosition(muzzle , bulletDir);
                bullet.Fire();
                var muzzleFlash = poolManagerSo.GetPoolObject<MuzzleFlash>();
                muzzleFlash.SetParent(muzzle);
                muzzleFlash.ResetPositionAndRotation();
                muzzleFlash.Play();
                SoundManager.PlaySound(AudioType.GunFire,muzzle.position);
            }
            
        }
        private void OnEnableWeapon(bool state)
        {
            mesh.SetActive(state);
        }
    }
    
    // public class 
}