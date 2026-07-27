using UnityEngine;

namespace Characters.Common
{
    public interface IPositioned
    {
        float FacingDirection { get; }
        Vector2 Position { get; }
    }
}
