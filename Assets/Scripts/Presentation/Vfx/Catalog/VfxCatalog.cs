using System;
using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Presentation.Vfx.Catalog
{
    [CreateAssetMenu(menuName = "LayerZero/Vfx/Vfx Catalog", fileName = "VfxCatalog")]
    internal sealed class VfxCatalog : ScriptableObject
    {
        [SerializeField] private VfxEntry[] _entries = Array.Empty<VfxEntry>();

        public IReadOnlyList<VfxEntry> Entries => _entries;
    }
}
