using LayerZero.Presentation.Vfx;
using LayerZero.Presentation.Vfx.Catalog;
using LayerZero.Presentation.Vfx.Combat;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LayerZero.Presentation.Container
{
    internal sealed class PresentationLifetimeScope : LifetimeScope
    {
        [SerializeField] private VfxCatalog _vfxCatalog;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_vfxCatalog);
            builder.Register<VfxService>(Lifetime.Singleton)
                .As<IVfxService>()
                .WithParameter(transform);

            builder.RegisterEntryPoint<AttackVfxPresenter>();
        }
    }
}
