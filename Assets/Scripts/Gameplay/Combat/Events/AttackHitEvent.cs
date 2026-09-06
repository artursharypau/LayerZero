using LayerZero.Core.EventBus.Events;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Events
{
    public readonly struct AttackHitEvent : IEventBusEvent
    {
        public readonly bool IsCritical;
        public readonly float Direction;
        public readonly Vector2 Position;

        public AttackHitEvent(bool isCritical, float direction, Vector2 position)
        {
            IsCritical = isCritical;
            Direction = direction;
            Position = position;
        }
    }
}
