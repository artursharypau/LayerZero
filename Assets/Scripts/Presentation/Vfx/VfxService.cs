using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    internal sealed class VfxService : IVfxService
    {
        private readonly Dictionary<VfxKind, IVfxPool> _pools;

        public VfxService(VfxCatalog catalog)
        {
            _pools = new Dictionary<VfxKind, IVfxPool>(catalog.Entries.Count);

            for (int i = 0; i < catalog.Entries.Count; i++)
            {
                VfxEntry entry = catalog.Entries[i];
                VfxInstance prefab = entry.Prefab;
                _pools[prefab.Kind] = new VfxPool(prefab, entry.PrewarmCount);
            }
        }

        public void Play(VfxKind kind, Vector2 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(kind, out IVfxPool pool))
            {
                return;
            }

            IVfxInstance instance = pool.Get();
            instance.Finished += OnVfxFinished;
            instance.Play();
        }

        private void OnVfxFinished(IVfxInstance instance)
        {
            instance.Finished -= OnVfxFinished;

            if (_pools.TryGetValue(instance.Kind, out IVfxPool pool))
            {
                instance.Disable();
                pool.Release(instance);
            }
        }

        public void Dispose()
        {
            foreach (IVfxPool pool in _pools.Values)
            {
                pool.Dispose();
            }

            _pools.Clear();
        }
    }
}
