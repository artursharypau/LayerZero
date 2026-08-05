using System;

namespace LayerZero.Core.StateMachine
{
    internal readonly struct StateTransition
    {
        private readonly int _target;
        private readonly Func<int> _resolveTarget;

        public readonly Func<bool> Condition;

        public StateTransition(Func<bool> condition, int target)
        {
            Condition = condition;
            _target = target;
            _resolveTarget = null;
        }

        public StateTransition(Func<bool> condition, Func<int> resolveTarget)
        {
            Condition = condition;
            _target = 0;
            _resolveTarget = resolveTarget;
        }

        public int ResolveTarget()
        {
            return _resolveTarget?.Invoke() ?? _target;
        }
    }
}
