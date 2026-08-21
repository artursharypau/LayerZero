using LayerZero.Characters.Common;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Characters.Player
{
    public sealed class PlayerLifetimeScope : CharacterLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.UseComponents(transform, components => components.AddInHierarchy<PlayerController>());
        }
    }
}
