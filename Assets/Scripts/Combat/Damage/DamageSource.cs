namespace LayerZero.Combat.Damage
{
    /// <summary>Who produced the damage. Used for aggro and for friendly-fire rules.</summary>
    public enum DamageSource
    {
        None = 0,
        Player = 1,
        Enemy = 2,
        Hazard = 3
    }
}
