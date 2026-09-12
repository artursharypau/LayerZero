using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Stats.Config;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Gameplay.Characters.Enemies
{
    internal sealed class EnemyLifetimeScope : CharacterLifetimeScope
    {
        protected override StatsConfig Stats => GetComponentInChildren<EnemyController>(true).Config.Stats;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.UseComponents(transform, components => components.AddInHierarchy<EnemyController>());
        }
    }
}
