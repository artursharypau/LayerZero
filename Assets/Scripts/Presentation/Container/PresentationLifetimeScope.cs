using LayerZero.Presentation.Vfx;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Presentation.Container
{
    public sealed class PresentationLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VfxService>(Lifetime.Singleton).As<IVfxService>();

            builder.RegisterComponentInHierarchy<AttackHitVfx>();
        }
    }
}
