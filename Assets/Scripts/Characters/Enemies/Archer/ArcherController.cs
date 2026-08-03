namespace LayerZero.Characters.Enemies.Archer
{
    /// <summary>
    /// An archer is a plain ranged enemy. Adding it takes an empty subclass, a
    /// RangedEnemyConfig asset, and a prefab carrying a ProjectileAttackExecutor
    /// component instead of a melee hitbox.
    /// </summary>
    public sealed class ArcherController : RangedEnemyController
    {
    }
}
