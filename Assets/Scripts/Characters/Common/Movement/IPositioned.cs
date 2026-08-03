using UnityEngine;

namespace LayerZero.Characters.Common.Movement
{
    public interface IPositioned
    {
        float FacingDirection { get; }
        Vector2 Position { get; }
        Vector2 FacingVector { get; }
    }
}
