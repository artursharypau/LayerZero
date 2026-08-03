namespace LayerZero.Characters.Enemies.States
{
    /// <summary>
    /// Planted shot. The projectile itself is spawned by the combat system on the animation's
    /// hit event, so this state only has to hold still, aim and yield to the recovery state.
    /// </summary>
    public sealed class EnemyRangedAttackState : EnemyAttackState
    {
        public EnemyRangedAttackState(EnemyController owner)
            : base(owner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Movement.Stop();
            Movement.FaceTowards(Owner.Perception.DirectionToTarget);
        }

        protected override void OnAttackFinished()
        {
            ChangeTo<EnemyRecoverState>();
        }
    }
}
