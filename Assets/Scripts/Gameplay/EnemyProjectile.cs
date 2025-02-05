using System;
using State_Machine.PlayerStateMachine.PlayerStates;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody)) , RequireComponent(typeof(BoxCollider))]
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float scaleInDuration = 1f;
        [SerializeField] float forwardForce = 100f;
        [SerializeField] int scaleMultiplier = 2;
        [SerializeField] private int damageAmount = 15;
        [SerializeField] GameObject projectile;
        [SerializeField] private Rigidbody rb;

        private void OnValidate()
        {
            if(rb == null) rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            projectile.transform.localScale = Vector3.one;
            LeanTween.scale(projectile , Vector3.one * scaleMultiplier , scaleInDuration).setOnComplete(() =>
            {
                var forwardDirection = transform.TransformDirection(Vector3.forward);
                rb.AddForce(forwardDirection * forwardForce, ForceMode.Impulse);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerStateMachine>();
            if(player == null || player.IsDead) return;
            
            player.TakeDamage(damageAmount);
            
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Reset()
        {
            rb.linearVelocity = Vector3.zero;
            transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
        }

        public void SetRotationAndPosition(Transform projectileSpawnPoint)
        {
            var forwardDirection = projectileSpawnPoint.TransformDirection(Vector3.forward);
            transform.position = projectileSpawnPoint.position;
            transform.rotation = quaternion.LookRotation(forwardDirection , Vector3.up);

        }
    }
}