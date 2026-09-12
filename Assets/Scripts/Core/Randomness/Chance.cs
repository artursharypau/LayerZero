using UnityEngine;

namespace LayerZero.Core.Randomness
{
    public static class Chance
    {
        public static bool Roll(float probability)
        {
            return probability > Random.Range(0f, 1f);
        }
    }
}
