using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create PoolManager", fileName = "PoolManager", order = 0)]
    public class PoolManager : ScriptableObject
    {
        const string KeySuffix = "ByType";
        Dictionary<string , AbstractPooling> pools = new Dictionary<string , AbstractPooling>();

        public void AddPool<T>( Func<T> factoryMethod, Action<T> turnOnCallback,
            Action<T> turnOffCallback, int poolSize = 0)
        {
            if (!pools.ContainsKey(typeof(T) + KeySuffix))
            {
                pools.Add(typeof(T) + KeySuffix , new ObjectPooling<T>(factoryMethod , turnOnCallback , turnOffCallback , poolSize));
            }
        }

        public void RemovePool<T>()
        {
            if (pools.ContainsKey(typeof(T) + KeySuffix))
            {
                pools.Remove(typeof(T) + KeySuffix);
            }
        }

        public ObjectPooling<T> GetPool<T>()
        {
            pools.TryGetValue(typeof(T) + KeySuffix, out AbstractPooling pool);
            return (ObjectPooling<T>)pool;
        }

        public T GetPoolObject<T>()
        {
            pools.TryGetValue(typeof(T) + KeySuffix, out AbstractPooling pool);
            return (((ObjectPooling<T>)pool)!).GetObject();
        }

        public void ReturnPoolObject<T>(T poolObject)
        {
            pools.TryGetValue(typeof(T) + KeySuffix, out AbstractPooling pool);
            (((ObjectPooling<T>)pool)!).ReturnObject(poolObject);
        }
    }
}