using LayerZero.Combat;
using LayerZero.Core.Events;
using LayerZero.Core.Extensions;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Characters.Common
{
    public sealed class CharacterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EventBus>(Lifetime.Scoped).As<IEventBus>();

            builder.RegisterComponent(this.GetRequiredComponentInChildren<CombatSystem>());
        }
    }
}
