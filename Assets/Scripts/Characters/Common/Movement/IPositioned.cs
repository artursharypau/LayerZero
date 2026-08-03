using UnityEngine;

namespace LayerZero.Characters.Common.Movement
{
    /// <summary>Where a character is and which way it looks. Enough for detectors and sensors.</summary>
    public interface IPositioned
    {
        /// <summary>-1 or +1.</summary>
        float FacingDirection { get; }

        Vector2 Position { get; }

        Vector2 FacingVector { get; }
    }
}
