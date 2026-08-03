using System;
using System.Collections.Generic;

namespace LayerZero.Core.StateMachine
{
    public sealed class StateRegistry
    {
        private readonly Dictionary<Type, StateBase> _exact = new();
        private readonly Dictionary<Type, StateBase> _aliases = new();
        private readonly HashSet<Type> _ambiguousAliases = new();

        public void Add(StateBase state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Type concrete = state.GetType();
            if (!_exact.TryAdd(concrete, state))
            {
                throw new InvalidOperationException($"State '{concrete.Name}' is already registered.");
            }

            for (Type ancestor = concrete.BaseType;
                 ancestor != null && ancestor != typeof(StateBase) && typeof(StateBase).IsAssignableFrom(ancestor);
                 ancestor = ancestor.BaseType)
            {
                if (!_aliases.TryAdd(ancestor, state))
                {
                    _ambiguousAliases.Add(ancestor);
                }
            }
        }

        public StateBase Resolve(Type key)
        {
            if (_exact.TryGetValue(key, out StateBase exact))
            {
                return exact;
            }

            if (_ambiguousAliases.Contains(key))
            {
                throw new InvalidOperationException(
                    $"'{key.Name}' is ambiguous: several registered states derive from it. Request a concrete state type instead.");
            }

            if (_aliases.TryGetValue(key, out StateBase alias))
            {
                return alias;
            }

            throw new KeyNotFoundException($"No state registered for '{key.Name}'.");
        }
    }
}
