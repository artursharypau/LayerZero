namespace LayerZero.Gameplay.Combat.Damage.Resistance
{
    public interface IDamageResistances
    {
        bool IsInvulnerable { get; }

        ResistanceHandle Apply(DamageResistance resistance);
        void Remove(ResistanceHandle handle);
        DamageImpactInfo Resolve(DamageImpactInfo impact);
    }
}
