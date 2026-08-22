using UnityEngine;

namespace LayerZero.Core.Events
{
    public readonly struct AttackHitEvent : IEventBusEvent
    {
        public readonly Transform Target;
        public readonly Vector2 Point;
        public readonly Vector2 Direction;

        public AttackHitEvent(Transform target, Vector2 point, Vector2 direction)
        {
            Target = target;
            Point = point;
            Direction = direction;
        }
    }
}
