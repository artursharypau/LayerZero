using LayerZero.Core.EventBus.Events;

namespace LayerZero.Core.EventBus.Handlers
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEventBusEvent
    {
        void Handle(TEvent e);
    }
}
