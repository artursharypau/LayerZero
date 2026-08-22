using LayerZero.Presentation.Vfx;
using LayerZero.Presentation.Vfx.Attack;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Presentation.Container
{
    public sealed class PresentationLifetimeScope : LifetimeScope
    {
        [SerializeField] private VfxCatalog _vfxCatalog;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VfxService>(Lifetime.Singleton).As<IVfxService>();

            builder.RegisterEntryPoint<AttackHitVfxPresenter>();
        }
    }
}
