using System;

namespace LayerZero.Gameplay.StatusEffects
{
    internal interface IStatusEffectHandler
    {
        Type EffectType { get; }
        bool IsActive { get; }

        void Remove(int id);
        void Clear();
        void Tick(float deltaTime);
    }

    internal interface IStatusEffectHandler<TEffect> : IStatusEffectHandler
        where TEffect : struct, IStatusEffect
    {
        bool TryApply(int id, in TEffect statusEffect);
    }
}
