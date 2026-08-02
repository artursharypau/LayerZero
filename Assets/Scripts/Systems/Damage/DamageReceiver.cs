using System;
using Systems.Combat;
using Systems.Damage.Resistance;
using UnityEngine;

namespace Systems.Damage
{
    [RequireComponent(typeof(Health))]
    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private CombatSystem _combatSystem;
        private IDamageable _damageable;
        private IDamageResistanceProvider _resistanceProvider;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> DamageImpactReceived;

        private void Awake()
        {
            _combatSystem = GetComponent<CombatSystem>();
            _damageable = GetComponent<IDamageable>();
            _resistanceProvider = GetComponent<IDamageResistanceProvider>();
        }

        private void OnEnable()
        {
            _combatSystem.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            _combatSystem.Damaged -= OnDamaged;
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            DamageResistance resistance = GetActiveResistance();
            if (resistance.IsInvulnerable)
            {
                return;
            }

            DamageImpactInfo resolvedDamageImpact = ResolveImpact(damageInfo.Impact, resistance);
            DamageInfo resolvedDamage = new(damageInfo.Amount, damageInfo.Source, damageInfo.AttackerTransform, resolvedDamageImpact);

            _damageable.TakeDamage(resolvedDamage.Amount);
            Damaged?.Invoke(resolvedDamage);

            if (resolvedDamageImpact.HasImpact)
            {
                DamageImpactReceived?.Invoke(resolvedDamageImpact);
            }
        }

        private static DamageImpactInfo ResolveImpact(DamageImpactInfo incoming, DamageResistance resistance)
        {
            if (!incoming.HasImpact)
            {
                return DamageImpactInfo.None;
            }

            Vector2 knockback = incoming.Knockback * resistance.KnockbackMultiplier;
            float stunDuration = resistance.CanBeStunned ? incoming.StunDuration : 0f;

            return new DamageImpactInfo(knockback, stunDuration);
        }

        private DamageResistance GetActiveResistance()
        {
            return _resistanceProvider?.GetActive() ?? DamageResistance.None;
        }
    }
}
