using UnityEngine;

namespace LayerZero.Characters.Common.Abilities
{
    public sealed class AbilityCharges
    {
        private readonly int _max;

        private int _available;

        public AbilityCharges(int max)
        {
            _max = Mathf.Max(0, max);
            _available = _max;
        }

        public bool HasCharges => _available > 0;

        public void Consume()
        {
            if (_available > 0)
            {
                --_available;
            }
        }

        public void Refill()
        {
            _available = _max;
        }

        public void RefillTo(int amount)
        {
            _available = Mathf.Clamp(amount, 0, _max);
        }
    }
}
