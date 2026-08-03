using LayerZero.Characters.Enemies.States;

namespace LayerZero.Characters.Enemies
{
    /// <summary>Enemies that walk up and swing. Base for skeletons, brutes, and anything similar.</summary>
    public abstract class MeleeEnemyController : EnemyController
    {
        protected override void RegisterCombatBehaviour()
        {
            States.Register(new EnemyChaseState(this));
            States.Register(new EnemyMeleeAttackState(this));
        }
    }
}
