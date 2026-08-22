using LayerZero.Core.EventBus.Events;
using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus.Subjects
{
    public interface IEventSubject
    {
    }

    public interface IEventSubject<TEvent> : IEventSubject
        where TEvent : IEventBusEvent
    {
        void AddHandler(IEventHandler<TEvent> handler);
        void RemoveHandler(IEventHandler<TEvent> handler);
        void Notify(TEvent e);
    }
}
