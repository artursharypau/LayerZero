namespace LayerZero.Gameplay.Characters.Enemies.States
{
    internal sealed class EnemyDeadState : EnemyState
    {
        public EnemyDeadState(EnemyController owner)
            : base(owner)
        {
        }

        public override int Id => EnemyStateId.Dead;
    }
}
