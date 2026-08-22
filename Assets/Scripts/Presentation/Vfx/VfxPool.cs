using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace LayerZero.Presentation.Vfx
{
    internal class VfxPool : IVfxPool
    {
        private readonly VfxInstance _prefab;

        private readonly List<VfxInstance> _all;
        private readonly Queue<IVfxInstance> _available;

        public VfxPool(VfxInstance prefab, int count)
        {
            _prefab = prefab;
            _all = new List<VfxInstance>(count);
            _available = new Queue<IVfxInstance>(count);

            for (int i = 0; i < count; i++)
            {
                VfxInstance instance = Object.Instantiate(prefab);

                _all.Add(instance);
                _available.Enqueue(instance);
            }
        }

        public IVfxInstance Get()
        {
            if (_available.Count == 0)
            {
                VfxInstance instance = Object.Instantiate(_prefab);
                _all.Add(instance);
                _available.Enqueue(instance);
            }

            IVfxInstance prefab = _available.Dequeue();
            prefab.Disable();

            return prefab;
        }

        public void Release(IVfxInstance instance)
        {
            if (_available.Contains(instance) && !_available.Contains(instance))
            {
                _available.Enqueue(instance);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _all.Count; i++)
            {
                Object.Destroy(_all[i].gameObject);
            }
        }
    }
}
