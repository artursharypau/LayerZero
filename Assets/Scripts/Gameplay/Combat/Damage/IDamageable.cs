namespace LayerZero.Gameplay.Combat.Damage
{
    internal interface IDamageable : IHealth
    {
        void TakeDamage(int amount);
    }
}
