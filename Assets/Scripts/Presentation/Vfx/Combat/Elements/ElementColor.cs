using System;
using LayerZero.Gameplay.Combat.Elements;
using UnityEngine;

namespace LayerZero.Presentation.Vfx.Combat.Elements
{
    [Serializable]
    internal sealed class ElementColor
    {
        [SerializeField] private ElementKind _kind;
        [SerializeField] private Color _color = Color.white;

        public ElementKind Kind => _kind;
        public Color Color => _color;
    }
}
