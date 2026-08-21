using System.Collections.Generic;
using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus.Subjects
{
    public class EventSubject : IEventSubject
    {
        private readonly List<IEventHandler> _handlers = new();

        public void AddHandler(IEventHandler handler)
        {
            _handlers.Add(handler);
        }

        public void RemoveHandler(IEventHandler handler)
        {
            _handlers.Remove(handler);
        }

        public void Notify(IEventBusEvent e)
        {
            foreach (IEventHandler handler in _handlers.ToArray())
            {
                handler.Handle(e);
            }
        }
    }
}
