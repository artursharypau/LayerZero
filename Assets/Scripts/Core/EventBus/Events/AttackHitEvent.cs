using UnityEngine;

namespace LayerZero.Core.EventBus.Events
{
    public readonly struct AttackHitEvent : IEventBusEvent
    {
        public readonly Vector2 Position;

        public AttackHitEvent(Vector2 position)
        {
            Position = position;
        }
    }
}
