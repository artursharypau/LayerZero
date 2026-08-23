using System;
using UnityEngine;

namespace LayerZero.Presentation.Vfx.Catalog
{
    [Serializable]
    internal sealed class VfxEntry
    {
        [SerializeField] private VfxInstance _prefab;
        [SerializeField] [Min(0)] private int _prewarmCount = 4;

        public VfxInstance Prefab => _prefab;
        public int PrewarmCount => _prewarmCount;
    }
}
