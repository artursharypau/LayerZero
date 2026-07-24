using System;
using UnityEngine;

namespace Characters.Player.Abilities
{
    [Serializable]
    public class PlayerChargeableAbility : IPlayerChargeableAbility
    {
        [SerializeField] private PlayerChargeableAbilityConfig _config;

        private ushort _available;

        public bool CanBeUsed(IPlayerAbilityContext context)
        {
            return _available > 0;
        }

        public void Trigger(IPlayerAbilityContext context)
        {
            if (_available > 0)
            {
                --_available;
            }
        }

        public IPlayerAbilityConfig GetConfig()
        {
            return _config;
        }

        public void Refill()
        {
            _available = _config.Charges;
        }
    }
}
