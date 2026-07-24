using System;
using Characters.Player.Input;
using UnityEngine;

namespace Characters.Player.Abilities.Jump
{
    [Serializable]
    public class PlayerJumpAbility : IPlayerChargeableAbility
    {
        [SerializeField] private PlayerJumpAbilityConfig _config;
        [SerializeField] private PlayerChargeableAbility _charge;

        public bool CanBeUsed(IPlayerAbilityContext context)
        {
            return _charge.CanBeUsed(context) && context.Input.WasPerformed(PlayerInputAction.Jump);
        }

        public void Trigger(IPlayerAbilityContext context)
        {
            context.Input.Consume(PlayerInputAction.Jump);
            _charge.Trigger(context);
        }

        public IPlayerAbilityConfig GetConfig()
        {
            return _config;
        }

        public void Refill()
        {
            _charge.Refill();
        }
    }
}
