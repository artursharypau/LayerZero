using LayerZero.Core.Events.Handlers;

namespace LayerZero.Core.Events
{
    public interface IEventBus
    {
        void Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEventBusEvent;

        void Unsubscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEventBusEvent;

        void Raise<TEvent>(TEvent e) where TEvent : IEventBusEvent;
    }
}
