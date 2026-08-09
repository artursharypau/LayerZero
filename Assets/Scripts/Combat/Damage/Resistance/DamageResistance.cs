namespace LayerZero.Combat.Damage.Resistance
{
    public readonly struct DamageResistance
    {
        public static readonly DamageResistance Invulnerability = new(ResistanceKind.Invulnerability);
        public static readonly DamageResistance StunImmunity = new(ResistanceKind.StunImmunity);

        public readonly ResistanceKind Kind;
        public readonly float Value;

        private DamageResistance(ResistanceKind kind, float value = 0f)
        {
            Kind = kind;
            Value = value;
        }

        public static DamageResistance Knockback(float multiplier)
        {
            return new DamageResistance(ResistanceKind.Knockback, multiplier);
        }
    }
}
