using System;
using LayerZero.Gameplay.Combat.Elements;
using UnityEngine;

namespace LayerZero.Presentation.Vfx.Combat.Elements
{
    [CreateAssetMenu(fileName = "ElementPalette", menuName = "LayerZero/Presentation/Element Palette")]
    internal sealed class ElementPalette : ScriptableObject
    {
        [SerializeField] private ElementColor[] _colors = Array.Empty<ElementColor>();

        public bool TryGetColor(ElementKind kind, out Color color)
        {
            color = default;

            if (kind == ElementKind.None)
            {
                return false;
            }

            for (int i = 0; i < _colors.Length; i++)
            {
                if (_colors[i].Kind == kind)
                {
                    color = _colors[i].Color;
                    return true;
                }
            }

            return false;
        }
    }
}
