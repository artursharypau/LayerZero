using Characters.Common.Movement;
using UnityEngine;

namespace Characters.Common.Extensions
{
    public static class PositionedExtensions
    {
        public static Vector2 GetFacingDirectionVector(this IPositioned positioned)
        {
            return new Vector2(positioned.FacingDirection, 0f);
        }
    }
}
