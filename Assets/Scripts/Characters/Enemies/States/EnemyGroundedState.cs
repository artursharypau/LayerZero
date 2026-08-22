namespace LayerZero.Characters.Enemies.States
{
    public abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(EnemyController owner)
            : base(owner)
        {
            On(() => Perception.HasTarget, EnemyStateId.Chase);
        }
    }
}
