using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayerZero.Presentation.Vfx.Pooling
{
    internal sealed class VfxPool : IVfxPool
    {
        private readonly VfxInstance _prefab;
        private readonly Transform _root;

        private readonly Queue<VfxInstance> _available;
        private readonly HashSet<VfxInstance> _inUse;

        public VfxPool(VfxInstance prefab, Transform root, int count)
        {
            _prefab = prefab;
            _root = root;

            _available = new Queue<VfxInstance>(count);
            _inUse = new HashSet<VfxInstance>(count);

            for (int i = 0; i < count; i++)
            {
                _available.Enqueue(Create());
            }
        }

        public IVfxInstance Get()
        {
            VfxInstance instance = _available.Count > 0 ? _available.Dequeue() : Create();
            _inUse.Add(instance);

            return instance;
        }

        public void Release(IVfxInstance instance)
        {
            if (instance is VfxInstance pooled && _inUse.Remove(pooled))
            {
                _available.Enqueue(pooled);
            }
        }

        public void Dispose()
        {
            foreach (VfxInstance instance in _available)
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            foreach (VfxInstance instance in _inUse)
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            _available.Clear();
            _inUse.Clear();
        }

        private VfxInstance Create()
        {
            VfxInstance instance = Object.Instantiate(_prefab, _root);
            instance.Disable();

            return instance;
        }
    }
}
