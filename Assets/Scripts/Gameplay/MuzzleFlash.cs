using System;
using System.Collections;
using ObjectPooling;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    public class MuzzleFlash : MonoBehaviour
    {
        PoolManager poolManager;    
        [SerializeField]ParticleSystem ps;

        private void OnValidate()
        {
            if(ps == null)ps = GetComponent<ParticleSystem>();  
        }

        public void Init(PoolManager poolManagerSo)
        {
            poolManager = poolManagerSo;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void ResetPositionAndRotation()
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
        }

        public void Play()
        {
            Enable(true);
            StartCoroutine(BackToPool(ps.main.duration));
        }

        public void Enable(bool b)
        {
            gameObject.SetActive(b);
        }

        IEnumerator BackToPool(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            poolManager.ReturnPoolObject(this);
        }
    }
}