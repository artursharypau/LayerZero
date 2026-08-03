using System;
using System.Collections.Generic;
using Systems.Damage.Resistance;
using UnityEngine;

namespace Systems.Damage
{
    [RequireComponent(typeof(IDamageable))]
    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private IDamageable _damageable;
        private IDamageResistanceApplier _resistanceApplier;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> DamageImpactReceived;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        public void SetDamageResistanceApplier(IDamageResistanceApplier resistanceApplier)
        {
            _resistanceApplier = resistanceApplier;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            List<DamageResistance> resistances = _resistanceApplier?.AppliedResistances;
            if (HasInvulnerability(resistances))
            {
                return;
            }

            DamageImpactInfo resolvedDamageImpact = ResolveImpact(damageInfo.Impact, resistances);
            DamageInfo resolvedDamage = new(damageInfo.Amount, damageInfo.Source, damageInfo.AttackerTransform, resolvedDamageImpact);

            _damageable.TakeDamage(resolvedDamage.Amount);
            Damaged?.Invoke(resolvedDamage);

            if (resolvedDamageImpact.HasImpact)
            {
                DamageImpactReceived?.Invoke(resolvedDamageImpact);
            }
        }

        private static bool HasInvulnerability(List<DamageResistance> resistances)
        {
            if (resistances == null)
            {
                return false;
            }

            for (int i = 0; i < resistances.Count; i++)
            {
                if (resistances[i].IsInvulnerable)
                {
                    return true;
                }
            }

            return false;
        }

        private static DamageImpactInfo ResolveImpact(DamageImpactInfo incoming, List<DamageResistance> resistances)
        {
            if (!incoming.HasImpact)
            {
                return DamageImpactInfo.None;
            }

            bool ignoresStun = false;
            float knockbackMultiplier = 1f;

            if (resistances != null && resistances.Count > 0)
            {
                for (int i = 0; i < resistances.Count; i++)
                {
                    DamageResistance resistance = resistances[i];
                    ignoresStun |= resistance.IgnoresStun;
                    knockbackMultiplier *= resistance.KnockbackReduceMultiplier;
                }
            }

            Vector2 knockback = incoming.Knockback * knockbackMultiplier;
            float stunDuration = ignoresStun ? 0f : incoming.StunDuration;

            return new DamageImpactInfo(knockback, stunDuration);
        }
    }
}
