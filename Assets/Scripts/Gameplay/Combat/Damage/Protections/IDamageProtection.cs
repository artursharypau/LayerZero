namespace LayerZero.Gameplay.Combat.Damage.Protections
{
    internal interface IDamageProtection
    {
        bool IsInvulnerable { get; }

        ProtectionHandle Apply(Protection protection);
        void Remove(ProtectionHandle handle);
        DamageImpactInfo Resolve(DamageImpactInfo impact);
    }
}
