using LayerZero.Core.EventBus.Events;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Events
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
