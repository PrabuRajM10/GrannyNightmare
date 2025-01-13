using System;
using ObjectPooling;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] private float thrust = 100f;
        PoolManager poolManager;

        private void OnValidate()
        {
            if(rb == null)rb = GetComponent<Rigidbody>(); 
        }
        private void OnTriggerEnter(Collider other)
        {
            var rifle = other.GetComponent<Rifle>();
            if(rifle) return;
            
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
