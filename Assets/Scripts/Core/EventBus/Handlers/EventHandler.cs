using System;

namespace LayerZero.Core.EventBus.Handlers
{
    public class EventHandler : IEventHandler
    {
        public void Handle(IEventBusEvent e)
        {
            throw new NotImplementedException();
        }
    }

    public class EventHandler<TEvent> : IEventHandler
        where TEvent : IEventBusEvent
    {
        private readonly Action<TEvent> _handler;

        public EventHandler(Action<TEvent> handler)
        {
            _handler = handler;
        }

        public void Handle(TEvent e)
        {
            _handler(e);
        }
    }
}
