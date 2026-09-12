namespace LayerZero.Gameplay.StatusEffects
{
    internal interface IStatusEffectsSystem
    {
        StatusEffectHandle Apply<TEffect>(in TEffect statusEffect)
            where TEffect : struct, IStatusEffect;

        void Remove(StatusEffectHandle handle);
    }
}
