using System;

namespace LayerZero.Presentation.Vfx.Pooling
{
    internal interface IVfxPool : IDisposable
    {
        IVfxInstance Get();
        void Release(IVfxInstance instance);
    }
}
