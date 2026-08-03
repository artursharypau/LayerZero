namespace LayerZero.Combat.Attacks
{
    /// <summary>
    /// How an attack reaches its target. Each kind is served by one
    /// <see cref="IAttackExecutor" /> component on the character.
    /// Adding a new delivery method (beam, ground shockwave) = one enum entry + one executor.
    /// </summary>
    public enum AttackKind
    {
        Melee = 0,
        Ranged = 1
    }
}
