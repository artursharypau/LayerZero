using System;

namespace LayerZero.Presentation.Vfx
{
    public interface IVfxInstance
    {
        event Action<IVfxInstance> Finished;

        public VfxKind Kind { get; }

        void Disable();
        void Play();
    }
}
