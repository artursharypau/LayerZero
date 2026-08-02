using System;

namespace Systems.Damage
{
    public interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> DamageImpactReceived;
    }
}
