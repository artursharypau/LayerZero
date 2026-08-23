using System;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    internal interface IVfxService : IDisposable
    {
        void Play(VfxKind kind, Vector2 position, Quaternion rotation);
    }
}
