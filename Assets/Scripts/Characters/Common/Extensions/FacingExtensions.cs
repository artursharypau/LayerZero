using UnityEngine;

namespace Characters.Common.Extensions
{
    public static class FacingExtensions
    {
        public static Vector2 GetVector(this IFacing facing)
        {
            return new Vector2(facing.FacingDirection, 0f);
        }
    }
}
