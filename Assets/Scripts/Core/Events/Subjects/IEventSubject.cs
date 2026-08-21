using LayerZero.Core.Events.Handlers;

namespace LayerZero.Core.Events.Subjects
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
