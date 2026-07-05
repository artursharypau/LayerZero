using UnityEngine;

namespace Common
{
    public static class LayerMaskProvider
    {
        public static LayerMask Ground = LayerMask.GetMask("Ground");
        public static LayerMask Player = LayerMask.GetMask("Player");
        public static LayerMask Enemy = LayerMask.GetMask("Enemy");

        public static bool Contains(int objectLayer, params LayerMask[] masks)
        {
            int combinedMask = 0;

            for (int i = 0; i < masks.Length; i++)
            {
                combinedMask |= masks[i].value;
            }

            return (combinedMask & (1 << objectLayer)) != 0;
        }
    }
}
