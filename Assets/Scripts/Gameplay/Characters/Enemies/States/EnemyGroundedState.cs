namespace LayerZero.Gameplay.Characters.Enemies.States
{
    internal abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(EnemyController owner)
            : base(owner)
        {
            On(() => Perception.HasTarget, EnemyStateId.Chase);
        }
    }
}
