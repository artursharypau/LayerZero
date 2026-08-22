using System.Collections.Generic;
using LayerZero.Core.Events.Handlers;

namespace LayerZero.Core.Events.Subjects
{
    public sealed class EventSubject<TEvent> : IEventSubject<TEvent>
        where TEvent : IEventBusEvent
    {
        private readonly List<IEventHandler<TEvent>> _handlers = new();

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _handlers.Add(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _handlers.Remove(handler);
        }

        public void Notify(TEvent e)
        {
            IEventHandler<TEvent>[] snapshot = _handlers.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                snapshot[i].Handle(e);
            }
        }
    }
}
