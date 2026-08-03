using LayerZero.Characters.Enemies.Config;
using LayerZero.Characters.Enemies.States;

namespace LayerZero.Characters.Enemies
{
    /// <summary>
    /// Enemies that fight at range: they hold a preferred distance, shoot, then recover.
    /// <para>
    /// Note what is not here - no perception, patrol, hurt or death code is repeated. The kiting
    /// chase is registered under the shared chase type, so states written for melee enemies keep
    /// working unchanged.
    /// </para>
    /// </summary>
    public abstract class RangedEnemyController : EnemyController
    {
        protected override void RegisterCombatBehaviour()
        {
            RangedEnemyConfig config = RequireConfig<RangedEnemyConfig>();
            if (!config)
            {
                return;
            }

            States.Register(new RangedEnemyChaseState(this, config.Ranged));
            States.Register(new EnemyRangedAttackState(this));
            States.Register(new EnemyRecoverState(this, config.Ranged.RecoveryDuration));
        }
    }
}
