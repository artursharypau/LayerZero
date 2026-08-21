using System.Collections.Generic;
using LayerZero.Core.EventBus.Handlers;
using LayerZero.Core.EventBus.Subjects;

namespace LayerZero.Core.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, IEventSubject> _subjects = new();

        public void Subscribe<TEvent>(IEventHandler handler)
            where TEvent : IEventBusEvent
        {
            string key = GetKey<TEvent>();
            if (!_subjects.TryGetValue(key, out IEventSubject subject))
            {
                subject = new EventSubject();
                _subjects.Add(key, subject);
            }

            subject.AddHandler(handler);
        }

        public void Unsubscribe<TEvent>(IEventHandler handler)
            where TEvent : IEventBusEvent
        {
            string key = GetKey<TEvent>();
            if (_subjects.TryGetValue(key, out IEventSubject subject))
            {
                subject.RemoveHandler(handler);
            }
        }

        public void Raise<TEvent>(TEvent e)
            where TEvent : IEventBusEvent
        {
            string key = GetKey<TEvent>();
            if (_subjects.TryGetValue(key, out IEventSubject subject))
            {
                subject.Notify(e);
            }
        }

        private static string GetKey<TEvent>()
            where TEvent : IEventBusEvent
        {
            return typeof(TEvent).FullName;
        }
    }
}
