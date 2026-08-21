namespace LayerZero.Core.Events.Handlers
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEventBusEvent
    {
        void Handle(TEvent e);
    }
}
