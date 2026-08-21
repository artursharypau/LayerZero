using System;
using LayerZero.Core.Disposable;
using LayerZero.Core.Events.Handlers;

namespace LayerZero.Core.Events
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
