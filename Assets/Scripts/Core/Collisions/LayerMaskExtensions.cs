using UnityEngine;

namespace LayerZero.Core.Collisions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        public static bool Contains(this LayerMask mask, GameObject gameObject)
        {
            return gameObject && mask.Contains(gameObject.layer);
        }

        public static LayerMask Or(this LayerMask mask, LayerMask fallback)
        {
            return mask.value != 0 ? mask : fallback;
        }
    }
}
