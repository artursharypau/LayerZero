using System;
using LayerZero.Core.Disposable;
using LayerZero.Core.EventBus.Events;
using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus
{
    public static class EventBusExtensions
    {
        public static DisposableSource SubscribeCallback<TEvent>(this IEventBus eventBus, Action<TEvent> action)
            where TEvent : IEventBusEvent
        {
            DelegateEventHandler<TEvent> handler = new(action);
            eventBus.Subscribe(handler);

            return new DisposableSource(() => eventBus.Unsubscribe(handler));
        }
    }
}
