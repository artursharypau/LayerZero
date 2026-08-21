using LayerZero.Characters.Common;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Characters.Enemies
{
    public sealed class EnemyLifetimeScope : CharacterLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.UseComponents(transform, components => components.AddInHierarchy<EnemyController>());
        }
    }
}
