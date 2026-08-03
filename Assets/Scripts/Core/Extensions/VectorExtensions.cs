using UnityEngine;

namespace LayerZero.Core.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 WithX(this Vector2 value, float x)
        {
            return new Vector2(x, value.y);
        }

        public static Vector2 WithY(this Vector2 value, float y)
        {
            return new Vector2(value.x, y);
        }

        public static Vector2 AlongFacing(this Vector2 value, float facingDirection)
        {
            return new Vector2(value.x * facingDirection, value.y);
        }
    }
}
