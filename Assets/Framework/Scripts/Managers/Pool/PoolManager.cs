using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Scripts.Managers.Pool
{
    public class PoolManager : BaseManager
    {
        private Transform _poolTransform;
        private Dictionary<Type, Stack<IPoolable>> _stacksByType;

        protected override void Awake()
        {
            base.Awake();

            _poolTransform = new GameObject("Pool").transform;
            _stacksByType = new Dictionary<Type, Stack<IPoolable>>();
        }

        public T GetGameObject<T>(GameObject prefab) where T : IPoolable
        {
            GameObject go;

            _stacksByType.TryGetValue(typeof(T), out var stack);

            if (stack?.Count > 0)
            {
                go = stack.Pop().GetGameObject();
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(prefab);
            }

            return go.GetComponent<T>();
        }

        public T GetGameObject<T>(GameObject prefab, Transform parent) where T : IPoolable
        {
            var poolable = GetGameObject<T>(prefab);
            poolable.GetGameObject().transform.SetParent(parent);
            return poolable;
        }

        public void ReleaseGameObject(IPoolable poolable)
        {
            var type = poolable.GetType();

            _stacksByType.TryGetValue(type, out var poolStack);

            if (poolStack == null)
            {
                poolStack = new Stack<IPoolable>();
                _stacksByType.Add(type, poolStack);
            }

            poolable.OnRelease();

            var go = poolable.GetGameObject();
            go.transform.SetParent(_poolTransform);
            go.SetActive(false);

            poolStack.Push(poolable);
        }
    }
}