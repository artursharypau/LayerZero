using System;
using LayerZero.Core.EventBus.Events;

namespace LayerZero.Core.EventBus.Handlers
{
    internal sealed class DelegateEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEventBusEvent
    {
        private readonly Action<TEvent> _handler;

        public DelegateEventHandler(Action<TEvent> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public void Handle(TEvent e)
        {
            _handler(e);
        }
    }
}
