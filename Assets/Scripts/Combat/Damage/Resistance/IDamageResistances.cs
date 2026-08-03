namespace LayerZero.Combat.Damage.Resistance
{
    public interface IDamageResistances
    {
        bool IsInvulnerable { get; }

        ResistanceHandle Apply(DamageResistance resistance);
        void Remove(ResistanceHandle handle);

        /// <summary>Applies every active resistance to an incoming impact.</summary>
        DamageImpactInfo Filter(DamageImpactInfo impact);
    }
}
