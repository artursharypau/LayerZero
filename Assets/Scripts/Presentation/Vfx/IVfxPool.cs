using System;

namespace LayerZero.Presentation.Vfx
{
    internal interface IVfxPool : IDisposable
    {
        IVfxInstance Get();
        void Release(IVfxInstance instance);
    }
}
