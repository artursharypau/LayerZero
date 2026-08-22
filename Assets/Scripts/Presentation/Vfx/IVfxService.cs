using UnityEngine;

namespace LayerZero.Presentation.Presentation.Vfx
{
    public interface IVfxService
    {
        void Play(GameObject go, Transform parent, Quaternion? rotation = null);
    }
}
