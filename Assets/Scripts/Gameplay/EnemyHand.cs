using System;
using State_Machine.EnemyStateMachine;
using UnityEngine;

namespace Gameplay
{
    public class EnemyHand : MonoBehaviour
    {
        [SerializeField] private int damage = 10;
        [SerializeField] BoxCollider boxCollider;

        private void OnValidate()
        {
            if(boxCollider == null) boxCollider = GetComponent<BoxCollider>();  
        }

        private void Start()
        {
            EnableAttack(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<EnemyStateMachine>();
            if(enemy != null) return;
            
            var damageable = other.GetComponent<IDamageable>();                        
            if (damageable != null)                                                    
            {                                                                          
                damageable.TakeDamage(damage);                                         
            }                                                                          
        }

        public void EnableAttack(bool state)
        {
            boxCollider.enabled = state;
        }
    }
}