using System.Collections.Generic;

namespace LayerZero.Combat.Damage.Resistance
{
    public sealed class DamageResistances : IDamageResistances
    {
        private readonly Dictionary<int, DamageResistance> _active = new();

        private int _nextId = 1;
        private int _invulnerabilityCount;

        public bool IsInvulnerable => _invulnerabilityCount > 0;

        public ResistanceHandle Apply(DamageResistance resistance)
        {
            int id = _nextId++;
            _active.Add(id, resistance);

            if (resistance.IsInvulnerable)
            {
                ++_invulnerabilityCount;
            }

            return new ResistanceHandle(id);
        }

        public void Remove(ResistanceHandle handle)
        {
            if (!_active.Remove(handle.Id, out DamageResistance resistance))
            {
                return;
            }

            if (resistance.IsInvulnerable)
            {
                --_invulnerabilityCount;
            }
        }

        public DamageImpactInfo Filter(DamageImpactInfo impact)
        {
            if (!impact.HasImpact || _active.Count == 0)
            {
                return impact;
            }

            bool ignoresStun = false;
            float knockbackMultiplier = 1f;

            foreach (DamageResistance resistance in _active.Values)
            {
                ignoresStun |= resistance.IgnoresStun;
                knockbackMultiplier *= resistance.KnockbackMultiplier;
            }

            return new DamageImpactInfo(
                impact.Knockback * knockbackMultiplier,
                ignoresStun ? 0f : impact.StunDuration);
        }
    }
}
