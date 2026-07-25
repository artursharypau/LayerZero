using UnityEngine;

namespace Characters.Player.Abilities.Chargeable
{
    public class PlayerCharges
    {
        private readonly int _charges;

        private int _available;

        public PlayerCharges(int charges)
        {
            _charges = charges;
            _available = charges;
        }

        public bool HasCharges => _available > 0;

        public void Consume()
        {
            if (_available > 0)
            {
                --_available;
            }
        }

        public void Refill(int amount)
        {
            _available = amount > 0 ? Mathf.Max(amount, _charges) : _charges;
        }
    }
}
