using System;
using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    internal sealed class VfxAnimatorEvents : MonoBehaviour, IVfxAnimatorEvents
    {
        public event Action VfxFinished;

        public void TriggerVfxFinished()
        {
            VfxFinished?.Invoke();
        }
    }
}
