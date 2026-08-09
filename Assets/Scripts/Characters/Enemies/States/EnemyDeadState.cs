namespace LayerZero.Characters.Enemies.States
{
    public class EnemyDeadState : EnemyState
    {
        public EnemyDeadState(EnemyController owner)
            : base(owner)
        {
        }

        public override int Id => EnemyStateId.Dead;

        public override void Enter()
        {
            base.Enter();

            Owner.Perception.ForgetTarget();
        }
    }
}
