using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus.Subjects
{
    public interface IEventSubject
    {
        void AddHandler(IEventHandler handler);
        void RemoveHandler(IEventHandler handler);
        void Notify(IEventBusEvent e);
    }
}
