using System;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    internal interface IVfxInstance
    {
        event Action<IVfxInstance> Finished;

        void Play(Vector2 position, Quaternion rotation, Color? tint = null);
        void Disable();
    }
}
