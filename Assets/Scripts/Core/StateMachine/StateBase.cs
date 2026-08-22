using System;
using System.Collections.Generic;

namespace LayerZero.Core.StateMachine
{
    public abstract class StateBase
    {
        private readonly List<StateTransition> _transitions = new();

        public abstract int Id { get; }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Exit()
        {
        }

        protected void On(Func<bool> condition, int target)
        {
            _transitions.Add(new StateTransition(condition, target));
        }

        protected void On(Func<bool> condition, Func<int> resolveTarget)
        {
            _transitions.Add(new StateTransition(condition, resolveTarget));
        }

        internal bool TryGetTransition(out int target)
        {
            return TryResolve(_transitions, out target);
        }

        private static bool TryResolve(List<StateTransition> transitions, out int target)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].Condition())
                {
                    target = transitions[i].ResolveTarget();
                    return true;
                }
            }

            target = -1;
            return false;
        }
    }
}
