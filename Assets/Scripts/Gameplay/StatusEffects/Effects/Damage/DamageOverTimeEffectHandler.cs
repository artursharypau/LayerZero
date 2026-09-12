using System;
using System.Collections.Generic;
using LayerZero.Gameplay.Combat.Damage.Processing;
using UnityEngine;

namespace LayerZero.Gameplay.StatusEffects.Effects.Damage
{
    internal sealed class DamageOverTimeEffectHandler : IStatusEffectHandler<DamageOverTimeEffect>
    {
        private struct ActiveEffect
        {
            internal readonly int Id;
            internal readonly float Amount;
            internal float Remaining;

            public ActiveEffect(int id, float amount, float remaining)
            {
                Id = id;
                Amount = amount;
                Remaining = remaining;
            }

            internal bool IsExpired => Remaining <= 0f;
        }

        private readonly IDamageReceiver _damageReceiver;

        private readonly List<ActiveEffect> _effects = new();

        public DamageOverTimeEffectHandler(IDamageReceiver damageReceiver)
        {
            _damageReceiver = damageReceiver;
        }

        public Type EffectType => typeof(DamageOverTimeEffect);
        public bool IsActive => _effects.Count > 0;

        public bool TryApply(int id, in DamageOverTimeEffect statusEffect)
        {
            if (statusEffect.Duration <= 0f || statusEffect.Amount <= 0f || _damageReceiver.IsDead)
            {
                return false;
            }

            _effects.Add(new ActiveEffect(id, statusEffect.Amount, statusEffect.Duration));
            return true;
        }

        public void Remove(int id)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Id == id)
                {
                    _effects.RemoveAt(i);
                    return;
                }
            }
        }

        public void Clear()
        {
            _effects.Clear();
        }

        public void Tick(float deltaTime)
        {
            if (!IsActive)
            {
                return;
            }

            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                ActiveEffect effect = _effects[i];

                float step = Mathf.Min(deltaTime, effect.Remaining);
                effect.Remaining -= step;

                _damageReceiver.TakePeriodicDamage(effect.Amount * step);

                if (_damageReceiver.IsDead)
                {
                    _effects.Clear();
                    return;
                }

                if (effect.IsExpired)
                {
                    _effects.RemoveAt(i);
                }
                else
                {
                    _effects[i] = effect;
                }
            }
        }
    }
}
