using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace LayerZero.Core.Pooling
{
    /// <summary>
    /// Thin typed wrapper over <see cref="ObjectPool{T}" /> for prefab-backed components
    /// (projectiles, hit sparks, pickups). Keeps instances parented under a single root.
    /// </summary>
    public sealed class ComponentPool<T> : IDisposable where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _root;
        private readonly ObjectPool<T> _pool;

        public ComponentPool(T prefab, Transform root, int prewarm = 0, int maxSize = 32)
        {
            _prefab = prefab ? prefab : throw new ArgumentNullException(nameof(prefab));
            _root = root;
            _pool = new ObjectPool<T>(Create, OnGet, OnRelease, OnDestroyInstance, true, Mathf.Max(1, prewarm), maxSize);
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T instance = _pool.Get();
            instance.transform.SetPositionAndRotation(position, rotation);
            return instance;
        }

        public void Release(T instance)
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

        private T Create()
        {
            return Object.Instantiate(_prefab, _root);
        }

        private static void OnGet(T instance)
        {
            instance.gameObject.SetActive(true);
        }

        private static void OnRelease(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        private static void OnDestroyInstance(T instance)
        {
            if (instance)
            {
                Object.Destroy(instance.gameObject);
            }
        }
    }
}
