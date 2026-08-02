using System;
using Systems.Combat;
using UnityEngine;

namespace Systems.Damage
{
    [RequireComponent(typeof(Health))]
    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private DamageResistance _damageResistance = new();

        private CombatSystem _combatSystem;
        private IDamageable _damageable;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> DamageImpactReceived;

        private void Awake()
        {
            _combatSystem = GetComponent<CombatSystem>();
            _damageable = GetComponent<IDamageable>();
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
            DamageImpactInfo resolvedDamageImpact = ResolveImpact(damageInfo.Impact);
            DamageInfo resolvedDamage = new(damageInfo.Amount, damageInfo.Source, damageInfo.AttackerTransform, resolvedDamageImpact);

            _damageable.TakeDamage(resolvedDamage.Amount);
            Damaged?.Invoke(resolvedDamage);

            if (resolvedDamageImpact.HasImpact)
            {
                DamageImpactReceived?.Invoke(resolvedDamageImpact);
            }
        }

        private DamageImpactInfo ResolveImpact(DamageImpactInfo incoming)
        {
            if (!incoming.HasImpact || IsInvulnerable())
            {
                return DamageImpactInfo.None;
            }

            float knockbackMultiplier = _damageResistance.KnockbackMultiplier;
            bool canBeStunned = _damageResistance.CanBeStunned;

            Vector2 knockback = incoming.Knockback * knockbackMultiplier;
            float stunDuration = canBeStunned ? incoming.StunDuration : 0f;

            return new DamageImpactInfo(knockback, stunDuration);
        }

        private bool IsInvulnerable()
        {
            return _damageResistance.IsInvulnerable;
        }
    }
}
