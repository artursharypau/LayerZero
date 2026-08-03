using System;
using LayerZero.Core.Diagnostics;

namespace LayerZero.Core.StateMachine
{
    /// <summary>
    /// Type-keyed state machine: owns the registry, the current state and the pending transition.
    /// Pure C#, no Unity lifecycle - the owner decides when to pump <see cref="Update" /> / <see cref="FixedUpdate" />.
    /// </summary>
    public sealed class StateMachine
    {
        private const int TransitionGuardThreshold = 8;

        private readonly StateRegistry _registry = new();

        private StateBase _pending;
        private bool _isFlushing;

        public StateBase Current { get; private set; }
        public bool IsRunning => Current != null;

        public TState Register<TState>(TState state) where TState : StateBase
        {
            _registry.Add(state);
            return state;
        }

        public void Start<TState>() where TState : StateBase
        {
            _pending = null;
            Current = _registry.Resolve(typeof(TState));
            Current.Enter();
        }

        public bool IsIn<TState>() where TState : StateBase
        {
            return Current is TState;
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
            _pending = state;

            if (mode == StateTransitionMode.Immediate)
            {
                FlushPending();
            }
        }

        private void FlushPending()
        {
            // Re-entrancy guard: an immediate transition requested from inside Enter()/Exit()
            // is picked up by the loop that is already running instead of nesting into it.
            if (_isFlushing)
            {
                return;
            }

            _isFlushing = true;
            int guard = 0;

            // Enter() may itself request another transition, so keep draining the queue:
            // Current must never end up being a state that already asked to be replaced.
            while (_pending != null)
            {
                if (guard++ >= TransitionGuardThreshold)
                {
                    GameLog.Warning(
                        this,
                        $"Transition guard ({TransitionGuardThreshold}) hit while leaving '{Current?.GetType().Name}'. " +
                        "Likely a transition loop - check TryTransition()/Enter().");

                    _pending = null;
                    break;
                }

                StateBase next = _pending;
                _pending = null;

                Current?.Exit();
                Current = next;
                Current.Enter();
            }

            _isFlushing = false;
        }
    }
}
