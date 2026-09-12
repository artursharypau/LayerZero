namespace LayerZero.Gameplay.StatusEffects.Effects.Slowdown
{
    internal readonly struct SlowdownOverTimeEffect : IStatusEffect
    {
        internal readonly float Value;
        internal readonly float Duration;

        public SlowdownOverTimeEffect(float value, float duration)
        {
            Value = value;
            Duration = duration;
        }
    }
}
