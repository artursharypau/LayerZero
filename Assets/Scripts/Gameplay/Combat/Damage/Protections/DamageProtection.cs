using System.Collections.Generic;

namespace LayerZero.Gameplay.Combat.Damage.Protections
{
    internal sealed class DamageProtection : IDamageProtection
    {
        private readonly Dictionary<int, Protection> _active = new();

        private int _nextId = 1;
        private int _invulnerabilityCount;

        public bool IsInvulnerable => _invulnerabilityCount > 0;

        public ProtectionHandle Apply(Protection protection)
        {
            int id = _nextId++;
            _active.Add(id, protection);

            if (protection.Kind == ProtectionKind.Invulnerability)
            {
                ++_invulnerabilityCount;
            }

            return new ProtectionHandle(id, protection.Kind);
        }

        public void Remove(ProtectionHandle handle)
        {
            if (!_active.Remove(handle.Id, out Protection protection))
            {
                return;
            }

            if (protection.Kind == ProtectionKind.Invulnerability)
            {
                --_invulnerabilityCount;
            }
        }

        public DamageImpactInfo Resolve(DamageImpactInfo impact)
        {
            if (!impact.HasImpact || _active.Count == 0)
            {
                return impact;
            }

            bool ignoresStun = false;
            float knockbackMultiplier = 1f;

            foreach (Protection protection in _active.Values)
            {
                switch (protection.Kind)
                {
                    case ProtectionKind.StunImmunity:
                        ignoresStun = true;
                        break;
                    case ProtectionKind.Knockback:
                        knockbackMultiplier *= protection.Value;
                        break;
                }
            }

            return new DamageImpactInfo(
                impact.Knockback * knockbackMultiplier,
                ignoresStun ? 0f : impact.StunDuration);
        }
    }
}
