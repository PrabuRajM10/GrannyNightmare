using System;
using ObjectPooling;
using State_Machine.EnemyStateMachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] private float thrust = 100f;
        [SerializeField] private int damage = 5;
        PoolManager poolManager;

        private void OnValidate()
        {
            if(rb == null)rb = GetComponent<Rigidbody>(); 
        }
        private void OnTriggerEnter(Collider other)
        {
            var rifle = other.GetComponent<Rifle>();
            if(rifle) return;

            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Vector3 lookDirection = transform.TransformDirection(-Vector3.forward);
                other.GetComponent<EnemyStateMachine>().LookAtDirection(lookDirection);
            }
            
            Debug.Log("[Bullet] [OnTriggerEnter] Triggered on "+ other.name);
            poolManager.ReturnPoolObject(this);
        }
        
        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        public void Fire()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            rb.AddForce(forward * thrust, ForceMode.Impulse);  
        }

        public void SetRotationAndPosition(Transform refTransform, Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.localRotation = targetRotation;
            transform.position = refTransform.position;
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
    }
}
