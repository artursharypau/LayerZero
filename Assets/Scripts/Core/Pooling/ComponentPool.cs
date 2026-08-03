using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace LayerZero.Core.Pooling
{
    public sealed class ComponentPool<TComponent> : IDisposable
        where TComponent : Component
    {
        private readonly TComponent _prefab;
        private readonly Transform _root;
        private readonly ObjectPool<TComponent> _pool;

        public ComponentPool(TComponent prefab, Transform root, int prewarm = 0, int maxSize = 32)
        {
            _prefab = prefab ? prefab : throw new ArgumentNullException(nameof(prefab));
            _root = root;
            _pool = new ObjectPool<TComponent>(Create, OnGet, OnRelease, OnDestroyInstance, true, Mathf.Max(1, prewarm), maxSize);
        }

        public TComponent Get(Vector3 position, Quaternion rotation)
        {
            TComponent instance = _pool.Get();
            instance.transform.SetPositionAndRotation(position, rotation);
            return instance;
        }

        public void Release(TComponent instance)
        {
            if (instance)
            {
                _pool.Release(instance);
            }
        }

        public void Dispose()
        {
            _pool.Dispose();
        }

        private TComponent Create()
        {
            return Object.Instantiate(_prefab, _root);
        }

        private static void OnGet(TComponent instance)
        {
            instance.gameObject.SetActive(true);
        }

        private static void OnRelease(TComponent instance)
        {
            instance.gameObject.SetActive(false);
        }

        private static void OnDestroyInstance(TComponent instance)
        {
            if (instance)
            {
                Object.Destroy(instance.gameObject);
            }
        }
    }
}
