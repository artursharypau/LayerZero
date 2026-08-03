namespace Systems.Damage.Resistance
{
    public struct DamageResistance
    {
        public bool IsInvulnerable { get; private set; }
        public bool IgnoresStun { get; private set; }
        public float KnockbackReduceMultiplier { get; private set; }

        public DamageResistance WithInvulnerability()
        {
            IsInvulnerable = true;
            return this;
        }

        public DamageResistance WithIgnoreStun()
        {
            IgnoresStun = true;
            return this;
        }

        public DamageResistance WithKnockbackReduceMultiplier(float multiplier)
        {
            KnockbackReduceMultiplier = multiplier;
            return this;
        }

        public static DamageResistance Create()
        {
            return new DamageResistance { KnockbackReduceMultiplier = 1f };
        }
    }
}
