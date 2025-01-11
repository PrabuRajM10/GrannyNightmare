using System;
using System.Collections.Generic;
using Gameplay;

namespace ObjectPooling
{
    public class ObjectPooling<T> : AbstractPooling
    {
        private List<T> pool;
        private Func<T> factoryMethod;
        private Action<T> turnOnCallback;
        private Action<T> turnOffCallback;
        
        public ObjectPooling (Func<T> factoryMethod , Action<T> turnOnCallback , Action<T> turnOffCallback , int initialPoolCount)
        {
            this.factoryMethod = factoryMethod;
            this.turnOnCallback = turnOnCallback;
            this.turnOffCallback = turnOffCallback; 
            
            pool = new List<T> ();

            for (int i = 0; i < initialPoolCount; i++)
            {
                var obj = factoryMethod();
                pool.Add (obj);
                turnOffCallback(obj);
            }
        }
        
        public T GetObject()
        {
           T result;
           if (pool.Count > 0)
           {
               result = pool[^1];
               pool.Remove(result);
           }
           else
           {
               result = factoryMethod();
           }

           turnOnCallback(result);
           return result;
        }

        public void ReturnObject(T obj)
        {
            turnOffCallback(obj);
            pool.Add(obj);
        }
    }
}
