using System.Collections.Generic;

namespace Systems.Damage.Resistance
{
    public interface IDamageResistanceApplier
    {
        List<DamageResistance> AppliedResistances { get; }

        int Apply(DamageResistance resistance);
        void Remove(int index);
    }
}
