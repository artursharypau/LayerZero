using LayerZero.Gameplay.Characters.Common;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Gameplay.Characters.Enemies
{
    internal sealed class EnemyLifetimeScope : CharacterLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.UseComponents(transform, components => components.AddInHierarchy<EnemyController>());
        }
    }
}
