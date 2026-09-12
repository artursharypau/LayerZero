using LayerZero.Core.EventBus.Events;
using LayerZero.Gameplay.Combat.Elements;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Events
{
    public readonly struct AttackHitEvent : IEventBusEvent
    {
        public readonly ElementKind Element;
        public readonly float Direction;
        public readonly Vector2 Position;

        public AttackHitEvent(ElementKind element, float direction, Vector2 position)
        {
            Element = element;
            Direction = direction;
            Position = position;
        }
    }
}
