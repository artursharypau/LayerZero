using System;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    internal interface IVfxInstance
    {
        event Action<IVfxInstance> Finished;

        VfxKind Kind { get; }

        void Play(Vector2 position, Quaternion rotation);
        void Disable();
    }
}
