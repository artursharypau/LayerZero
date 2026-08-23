using System.Collections.Generic;
using LayerZero.Core.EventBus.Events;
using LayerZero.Core.EventBus.Handlers;
using LayerZero.Core.EventBus.Subjects;

namespace LayerZero.Core.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, IEventSubject> _subjects = new();

        public void Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEventBusEvent
        {
            string key = GetKey<TEvent>();

            if (!_subjects.TryGetValue(key, out IEventSubject subject))
            {
                subject = new EventSubject<TEvent>();
                _subjects.Add(key, subject);
            }

            ((IEventSubject<TEvent>)subject).AddHandler(handler);
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEventBusEvent
        {
            if (_subjects.TryGetValue(GetKey<TEvent>(), out IEventSubject subject))
            {
                ((IEventSubject<TEvent>)subject).RemoveHandler(handler);
            }
        }

        public void Raise<TEvent>(TEvent e)
            where TEvent : IEventBusEvent
        {
            if (_subjects.TryGetValue(GetKey<TEvent>(), out IEventSubject subject))
            {
                ((IEventSubject<TEvent>)subject).Notify(e);
            }
        }

        private static string GetKey<TEvent>()
            where TEvent : IEventBusEvent
        {
            return typeof(TEvent).FullName;
        }
    }
}
