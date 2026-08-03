using System;
using System.Collections.Generic;
using LayerZero.Core.Diagnostics;

namespace LayerZero.Characters.Common.Abilities
{
    /// <summary>
    /// Character module that owns a character's abilities and ticks the ones that need it.
    /// Keyed by an archetype-specific enum, so each character family declares its own ability set
    /// without a shared registry to extend.
    /// </summary>
    public class AbilitySet<TId> : CharacterModule where TId : struct, Enum
    {
        private readonly Dictionary<TId, IAbility> _abilities = new();
        private readonly List<ITickableAbility> _tickable = new();

        public AbilitySet<TId> Add(TId id, IAbility ability)
        {
            if (ability == null)
            {
                throw new ArgumentNullException(nameof(ability));
            }

            _abilities[id] = ability;

            if (ability is ITickableAbility tickable)
            {
                _tickable.Add(tickable);
            }

            return this;
        }

        public bool CanUse(TId id)
        {
            return TryGet(id, out IAbility ability) && ability.CanUse();
        }

        /// <summary>Uses the ability if it is available. Returns whether it actually fired.</summary>
        public bool TryUse(TId id)
        {
            if (!TryGet(id, out IAbility ability) || !ability.CanUse())
            {
                return false;
            }

            ability.Use();
            return true;
        }

        public void Refill(TId id)
        {
            if (TryGetChargeable(id, out IChargeableAbility ability))
            {
                ability.Refill();
            }
        }

        public void RefillTo(TId id, int amount)
        {
            if (TryGetChargeable(id, out IChargeableAbility ability))
            {
                ability.RefillTo(amount);
            }
        }

        public override void Tick(float deltaTime)
        {
            for (int i = 0; i < _tickable.Count; i++)
            {
                _tickable[i].Tick(deltaTime);
            }
        }

        private bool TryGet(TId id, out IAbility ability)
        {
            if (_abilities.TryGetValue(id, out ability))
            {
                return true;
            }

            GameLog.Error(this, $"Ability '{id}' is not registered.");
            return false;
        }

        private bool TryGetChargeable(TId id, out IChargeableAbility chargeable)
        {
            if (TryGet(id, out IAbility ability) && ability is IChargeableAbility found)
            {
                chargeable = found;
                return true;
            }

            chargeable = null;
            return false;
        }
    }
}
