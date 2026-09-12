using System.Collections.Generic;
using LayerZero.Presentation.Vfx.Combat.Attack;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayerZero.Presentation.Vfx.Pooling
{
    internal sealed class VfxPool : IVfxPool
    {
        private readonly AttackVfxInstance _prefab;
        private readonly Transform _root;

        private readonly Queue<AttackVfxInstance> _available;
        private readonly HashSet<AttackVfxInstance> _inUse;

        public VfxPool(AttackVfxInstance prefab, Transform root, int count)
        {
            _prefab = prefab;
            _root = root;

            _available = new Queue<AttackVfxInstance>(count);
            _inUse = new HashSet<AttackVfxInstance>(count);

            for (int i = 0; i < count; i++)
            {
                _available.Enqueue(Create());
            }
        }

        public IVfxInstance Get()
        {
            AttackVfxInstance instance = _available.Count > 0 ? _available.Dequeue() : Create();
            _inUse.Add(instance);

            return instance;
        }

        public void Release(IVfxInstance instance)
        {
            if (instance is AttackVfxInstance pooled && _inUse.Remove(pooled))
            {
                _available.Enqueue(pooled);
            }
        }

        public void Dispose()
        {
            foreach (AttackVfxInstance instance in _available)
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            foreach (AttackVfxInstance instance in _inUse)
            {
                if (instance)
                {
                    Object.Destroy(instance.gameObject);
                }
            }

            _available.Clear();
            _inUse.Clear();
        }

        private AttackVfxInstance Create()
        {
            AttackVfxInstance instance = Object.Instantiate(_prefab, _root);
            instance.Disable();

            return instance;
        }
    }
}
