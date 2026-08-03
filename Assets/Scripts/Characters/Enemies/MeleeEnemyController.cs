using LayerZero.Characters.Enemies.States;

namespace LayerZero.Characters.Enemies
{
    public abstract class MeleeEnemyController : EnemyController
    {
        protected override void RegisterCombatBehaviour()
        {
            StateMachine.Register(new EnemyChaseState(this));
            StateMachine.Register(new EnemyMeleeAttackState(this));
        }
    }
}
