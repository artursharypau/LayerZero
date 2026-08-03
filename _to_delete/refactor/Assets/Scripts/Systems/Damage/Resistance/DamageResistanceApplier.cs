using System.Collections.Generic;

namespace Systems.Damage.Resistance
{
    public class DamageResistanceApplier : IDamageResistanceApplier
    {
        private int _index;

        public List<DamageResistance> AppliedResistances { get; } = new();

        public int Apply(DamageResistance resistance)
        {
            AppliedResistances.Add(resistance);
            return _index++;
        }

        public void Remove(int index)
        {
            if (index >= 0 && index < AppliedResistances.Count)
            {
                AppliedResistances.RemoveAt(index);
                --_index;
            }
        }
    }
}
