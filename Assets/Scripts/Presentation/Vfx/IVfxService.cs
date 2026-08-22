using UnityEngine;

namespace LayerZero.Presentation.Vfx
{
    public interface IVfxService
    {
        void Play(GameObject prefab, Vector2 position, Quaternion rotation);
    }
}
