namespace LayerZero.Gameplay.StatusEffects.Effects.Damage
{
    internal readonly struct DamageOverTimeEffect : IStatusEffect
    {
        internal readonly float Amount;
        internal readonly float Duration;

        public DamageOverTimeEffect(float amount, float duration)
        {
            Amount = amount;
            Duration = duration;
        }
    }
}
