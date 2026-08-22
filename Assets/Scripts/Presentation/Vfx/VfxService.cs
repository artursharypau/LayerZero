using System.Collections.Generic;
using LayerZero.Presentation.Vfx.Catalog;
using LayerZero.Presentation.Vfx.Pooling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayerZero.Presentation.Vfx
{
    internal sealed class VfxService : IVfxService
    {
        private const string RootName = "[Vfx]";

        private readonly Dictionary<VfxKind, IVfxPool> _pools;
        private readonly Transform _root;

        public VfxService(VfxCatalog catalog, Transform parent)
        {
            _root = new GameObject(RootName).transform;
            _root.SetParent(parent);

            _pools = new Dictionary<VfxKind, IVfxPool>(catalog.Entries.Count);
            for (int i = 0; i < catalog.Entries.Count; i++)
            {
                Register(catalog.Entries[i]);
            }
        }

        public void Play(VfxKind kind, Vector2 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(kind, out IVfxPool pool))
            {
                Debug.LogWarning($"Vfx '{kind}' is not registered in the catalog.");
                return;
            }

            IVfxInstance instance = pool.Get();

            instance.Finished += OnVfxFinished;
            instance.Play(position, rotation);
        }

        public void Dispose()
        {
            foreach (IVfxPool pool in _pools.Values)
            {
                pool.Dispose();
            }

            _pools.Clear();

            if (_root)
            {
                Object.Destroy(_root.gameObject);
            }
        }

        private void Register(VfxEntry entry)
        {
            VfxInstance prefab = entry.Prefab;

            if (!prefab)
            {
                Debug.LogError("Vfx catalog contains an entry without a prefab.");
                return;
            }

            if (prefab.Kind == VfxKind.None)
            {
                Debug.LogError($"Vfx prefab '{prefab.name}' has no kind assigned.", prefab);
                return;
            }

            if (_pools.ContainsKey(prefab.Kind))
            {
                Debug.LogError($"Vfx catalog contains a duplicate entry for '{prefab.Kind}'.", prefab);
                return;
            }

            _pools.Add(prefab.Kind, new VfxPool(prefab, _root, entry.PrewarmCount));
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
    }
}
