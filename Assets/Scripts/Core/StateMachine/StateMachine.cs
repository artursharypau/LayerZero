using System;
using LayerZero.Core.Diagnostics;

namespace LayerZero.Core.StateMachine
{
    public sealed class StateMachine
    {
        private const int TransitionGuardThreshold = 8;

        private readonly StateRegistry _registry = new();

        private bool _isFlushing;

        public StateBase Pending { get; private set; }
        public StateBase Current { get; private set; }

        public void Register<TState>(TState state) where TState : StateBase
        {
            _registry.Add(state);
        }

        public void Start<TState>() where TState : StateBase
        {
            Pending = null;
            Current = _registry.Resolve(typeof(TState));
            Current.Enter();
        }

        public void ChangeState<TState>(StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            Schedule(_registry.Resolve(typeof(TState)), mode);
        }

        public void ChangeState<TState, TPayload>(TPayload payload, StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            StateBase state = _registry.Resolve(typeof(TState));
            if (state is not IStatePayload<TPayload> sink)
            {
                throw new InvalidOperationException(
                    $"State '{state.GetType().Name}' does not accept a payload of type '{typeof(TPayload).Name}'.");
            }

            sink.SetPayload(payload);
            Schedule(state, mode);
        }

        public void Update()
        {
            FlushPending();

            if (Current == null)
            {
                return;
            }

            if (!Current.TryTransition())
            {
                Current.Update();
            }
        }

        public void FixedUpdate()
        {
            FlushPending();

            if (Current == null)
            {
                return;
            }

            if (!Current.TryFixedTransition())
            {
                Current.FixedUpdate();
            }
        }

        private void Schedule(StateBase state, StateTransitionMode mode)
        {
            Pending = state;

            if (mode == StateTransitionMode.Immediate)
            {
                FlushPending();
            }
        }

        private void FlushPending()
        {
            if (_isFlushing)
            {
                return;
            }

            _isFlushing = true;
            int guard = 0;

            // Enter() may itself request another transition, so keep draining the queue:
            // Current must never end up being a state that already asked to be replaced.
            while (Pending != null)
            {
                if (guard++ >= TransitionGuardThreshold)
                {
                    GameLog.Warning(
                        this,
                        $"Transition guard ({TransitionGuardThreshold}) hit while leaving '{Current?.GetType().Name}'. "
                        + "Likely a transition loop - check TryTransition()/Enter().");

                    Pending = null;
                    break;
                }

                StateBase next = Pending;
                Pending = null;

                Current?.Exit();
                Current = next;
                Current.Enter();
            }

            _isFlushing = false;
        }
    }
}
