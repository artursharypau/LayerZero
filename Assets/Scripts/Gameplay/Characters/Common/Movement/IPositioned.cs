using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Movement
{
    public interface IPositioned
    {
        event Action<float> FacingDirectionChanged;

        float FacingDirection { get; }
        Vector2 Position { get; }
        Vector2 FacingVector { get; }
    }
}
