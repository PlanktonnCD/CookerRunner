using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [Serializable]
    public class MonoBehaviourPool<T> : BasePool<T> where T : MonoBehaviour
    {
        protected List<T> _poolList = new List<T>();

        public override T GetObject()
        {
            foreach (var poolObject in _poolList)
            {
                if (poolObject.gameObject.activeSelf == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }
            var newPoolObject = MonoBehaviour.Instantiate(prefab, parent.transform).GetComponent<T>();
            _poolList.Add(newPoolObject);
            return newPoolObject;
        }

        public override void ReturnObject(T poolObject)
        { 
            poolObject.gameObject.SetActive(false);
        }

        public IReadOnlyList<T> GetAllActiveObjects()
        {
            var newList = new List<T>();
            foreach (var poolObject in _poolList)
            {
                if (poolObject.gameObject.activeSelf == true)
                {
                    newList.Add(poolObject);
                }
            }
            return newList;
        }

        public void ReturnAll()
        {
            foreach (var monoBehaviour in _poolList)
            {
                ReturnObject(monoBehaviour);
            }
        }

        
        public void ReleaseAll()
        {
            foreach (var monoBehaviour in _poolList)
            {
                ((IPoolable)monoBehaviour).Release();
            }
        }
    }
}