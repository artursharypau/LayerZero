using LayerZero.Gameplay.Characters.Common.States;

namespace LayerZero.Gameplay.Characters.Enemies.States
{
    public sealed class EnemyMeleeAttackState : EnemyState
    {
        private readonly AttackBehaviour _attack;

        public EnemyMeleeAttackState(EnemyController owner)
            : base(owner)
        {
            _attack = new AttackBehaviour(owner, () => Config.Attack);

            On(() => _attack.IsFinished, EnemyStateId.Chase);
        }

        public override int Id => EnemyStateId.Attack;

        public override void Enter()
        {
            base.Enter();

            Movement.SetVelocityX(0f);

            _attack.Begin();
        }

        public override void Exit()
        {
            base.Exit();

            _attack.End();
        }
    }
}
