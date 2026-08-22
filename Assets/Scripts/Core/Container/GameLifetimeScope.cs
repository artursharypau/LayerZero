using LayerZero.Core.EventBus;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Core.Container
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameEventBus>(Lifetime.Singleton).As<IGameEventBus>();
        }
    }
}
