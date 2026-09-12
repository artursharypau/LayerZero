using System;

namespace LayerZero.Gameplay.StatusEffects
{
    internal readonly struct StatusEffectHandle
    {
        internal readonly Type EffectType;
        internal readonly int Id;

        public StatusEffectHandle(Type effectType, int id)
        {
            EffectType = effectType;
            Id = id;
        }

        internal bool IsValid => EffectType != null;
    }
}
