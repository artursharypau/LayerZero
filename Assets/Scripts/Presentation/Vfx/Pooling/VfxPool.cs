using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayerZero.Presentation.Vfx.Pooling
{
    internal sealed class VfxPool : IVfxPool
    {
        private readonly VfxInstance _prefab;
        private readonly Transform _root;

        private readonly List<VfxInstance> _all;
        private readonly Queue<IVfxInstance> _available;

        public VfxPool(VfxInstance prefab, Transform root, int count)
        {
            _prefab = prefab;
            _root = root;

            _all = new List<VfxInstance>(count);
            _available = new Queue<IVfxInstance>(count);

            for (int i = 0; i < count; i++)
            {
                VfxInstance instance = Create();

                _all.Add(instance);
                _available.Enqueue(instance);
            }
        }

        public IVfxInstance Get()
        {
            if (_available.Count == 0)
            {
                VfxInstance instance = Create();

                _all.Add(instance);
                _available.Enqueue(instance);
            }

            return _available.Dequeue();
        }

        public void Release(IVfxInstance instance)
        {
            if (_all.Contains(instance as VfxInstance) && !_available.Contains(instance))
            {
                _available.Enqueue(instance);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _all.Count; i++)
            {
                if (_all[i])
                {
                    Object.Destroy(_all[i].gameObject);
                }
            }

            _all.Clear();
            _available.Clear();
        }

        private VfxInstance Create()
        {
            VfxInstance instance = Object.Instantiate(_prefab, _root);
            instance.Disable();

            return instance;
        }
    }
}
