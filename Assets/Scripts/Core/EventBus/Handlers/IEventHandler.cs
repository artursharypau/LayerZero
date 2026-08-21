namespace LayerZero.Core.EventBus.Handlers
{
    public interface IEventHandler
    {
        void Handle(IEventBusEvent e);
    }
}
