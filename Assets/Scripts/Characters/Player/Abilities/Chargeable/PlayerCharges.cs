using UnityEngine;

namespace Characters.Player.Abilities.Chargeable
{
    public class PlayerCharges
    {
        private readonly int _max;

        private int _available;

        public PlayerCharges(int max)
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
