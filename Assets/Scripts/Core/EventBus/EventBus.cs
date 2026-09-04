using System;
using System.Collections.Generic;
using LayerZero.Core.EventBus.Events;
using LayerZero.Core.EventBus.Handlers;
using LayerZero.Core.EventBus.Subjects;

namespace LayerZero.Core.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, IEventSubject> _subjects = new();

        public void Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEventBusEvent
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Type key = typeof(TEvent);

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
            if (_subjects.TryGetValue(typeof(TEvent), out IEventSubject subject))
            {
                ((IEventSubject<TEvent>)subject).RemoveHandler(handler);
            }
        }

        public void Raise<TEvent>(TEvent e)
            where TEvent : IEventBusEvent
        {
            if (_subjects.TryGetValue(typeof(TEvent), out IEventSubject subject))
            {
                ((IEventSubject<TEvent>)subject).Notify(e);
            }
        }
    }
}
