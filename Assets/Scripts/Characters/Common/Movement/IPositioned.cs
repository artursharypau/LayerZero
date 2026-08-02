using UnityEngine;

namespace Characters.Common.Movement
{
    public interface IPositioned
    {
        float FacingDirection { get; }
        Vector2 Position { get; }
    }
}
