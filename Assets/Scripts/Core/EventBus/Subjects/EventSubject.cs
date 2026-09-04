using System;
using System.Collections.Generic;
using LayerZero.Core.EventBus.Events;
using LayerZero.Core.EventBus.Handlers;

namespace LayerZero.Core.EventBus.Subjects
{
    internal sealed class EventSubject<TEvent> : IEventSubject<TEvent>
        where TEvent : IEventBusEvent
    {
        private readonly List<IEventHandler<TEvent>> _handlers = new();

        private bool _isSnapshotStale;
        private IEventHandler<TEvent>[] _snapshot = Array.Empty<IEventHandler<TEvent>>();

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _handlers.Add(handler);
            _isSnapshotStale = true;
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            if (_handlers.Remove(handler))
            {
                _isSnapshotStale = true;
            }
        }

        public void Notify(TEvent e)
        {
            if (_isSnapshotStale)
            {
                _snapshot = _handlers.ToArray();
                _isSnapshotStale = false;
            }

            for (int i = 0; i < _snapshot.Length; i++)
            {
                _snapshot[i].Handle(e);
            }
        }
    }
}
