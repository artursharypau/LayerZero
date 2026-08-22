using System.Collections.Generic;
using System.Linq;

namespace LayerZero.Gameplay.Combat.Damage.Resistance
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

            if (resistance.Kind == ResistanceKind.Invulnerability)
            {
                ++_invulnerabilityCount;
            }

            return new ResistanceHandle(id, resistance.Kind);
        }

        public void Remove(ResistanceHandle handle)
        {
            if (!_active.Remove(handle.Id, out DamageResistance resistance))
            {
                return;
            }

            if (resistance.Kind == ResistanceKind.Invulnerability)
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

            for (int i = 0; i < _active.Values.Count; i++)
            {
                DamageResistance resistance = _active.Values.ElementAt(i);

                switch (resistance.Kind)
                {
                    case ResistanceKind.StunImmunity:
                        ignoresStun = true;
                        break;
                    case ResistanceKind.Knockback:
                        knockbackMultiplier *= resistance.Value;
                        break;
                }
            }

            return new DamageImpactInfo(
                impact.Knockback * knockbackMultiplier,
                ignoresStun ? 0f : impact.StunDuration);
        }
    }
}
