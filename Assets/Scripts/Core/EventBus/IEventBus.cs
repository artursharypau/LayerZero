using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus
{
    public interface IEventBus
    {
        void Subscribe<TEvent>(IEventHandler handler) where TEvent : IEventBusEvent;
        void Unsubscribe<TEvent>(IEventHandler handler) where TEvent : IEventBusEvent;
        void Raise<TEvent>(TEvent e) where TEvent : IEventBusEvent;
    }
}
