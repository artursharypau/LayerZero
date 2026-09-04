using UnityEngine;

namespace LayerZero.Core.Randomness
{
    public static class Chance
    {
        public static bool Roll(float percent)
        {
            return percent > Random.Range(0f, 100f);
        }
    }
}
