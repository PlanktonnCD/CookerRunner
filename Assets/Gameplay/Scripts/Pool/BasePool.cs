using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pool
{
    public abstract class BasePool<T>
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected GameObject parent;
        
        public abstract T GetObject();
        public abstract void ReturnObject(T poolObject);
    }
}