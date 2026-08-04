using System;
using System.Collections.Generic;

namespace LayerZero.Core.StateMachine
{
    public sealed class StateRegistry
    {
        private readonly Dictionary<int, StateBase> _states = new(8);

        public void Add(StateBase state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            _states[state.Id] = state;
        }

        public StateBase Get(int id)
        {
            if (!_states.TryGetValue(id, out StateBase state))
            {
                throw new KeyNotFoundException($"No state registered for id {id}.");
            }

            return state;
        }
    }
}
