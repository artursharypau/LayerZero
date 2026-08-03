using UnityEngine;

namespace LayerZero.Characters.Common.Abilities
{
    public sealed class AbilityCharges
    {
        public AbilityCharges(int max)
        {
            Max = Mathf.Max(0, max);
            Available = Max;
        }

        public int Available { get; private set; }

        public int Max { get; }

        public bool HasCharges => Available > 0;

        public void Consume()
        {
            if (Available > 0)
            {
                --Available;
            }
        }

        public void Refill()
        {
            Available = Max;
        }

        public void RefillTo(int amount)
        {
            Available = Mathf.Clamp(amount, 0, Max);
        }
    }
}
