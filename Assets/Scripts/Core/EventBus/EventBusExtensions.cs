using System;
using LayerZero.Core.Core.Disposable;

namespace LayerZero.Core.EventBus
{
    public static class EventBusExtensions
    {
        public static DisposableSource SubscribeCallback<TEvent>(this IEventBus bus, Action<TEvent> action)
            where TEvent : IEventBusEvent
        {
            Handlers.EventHandler<TEvent> handler = new Handlers.EventHandler<TEvent>(action);
            bus.Subscribe<TEvent>(handler);

            return new DisposableSource(() => bus.Unsubscribe<TEvent>(handler));
        }
    }
}
