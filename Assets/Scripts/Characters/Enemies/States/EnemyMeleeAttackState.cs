namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyMeleeAttackState : EnemyAttackState
    {
        public EnemyMeleeAttackState(EnemyController owner)
            : base(owner)
        {
        }

        public override int Id => EnemyStateId.Attack;

        public override void Enter()
        {
            base.Enter();

            Movement.SetVelocityX(0f);
        }
    }
}
