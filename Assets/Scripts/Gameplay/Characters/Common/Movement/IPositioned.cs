using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Movement
{
    public interface IPositioned
    {
        float FacingDirection { get; }
        Vector2 Position { get; }
        Vector2 FacingVector { get; }
    }
}
