using System;
using LayerZero.Core.Diagnostics;

namespace LayerZero.Core.StateMachine
{
    public sealed class StateMachine
    {
        private const int TransitionGuardThreshold = 8;

        private readonly StateRegistry _registry = new();

        private bool _isFlushing;

        public event Action<StateBase> StateEntered;

        public bool Running { get; private set; }
        public StateBase Pending { get; private set; }
        public StateBase Current { get; private set; }

        public void Register(StateBase state)
        {
            _registry.Add(state);
        }

        public void Start(int initialId)
        {
            Running = true;
            Pending = null;
            Current = _registry.Get(initialId);
            Current.Enter();

            StateEntered?.Invoke(Current);
        }

        public void Stop()
        {
            FlushPending();

            Running = false;
        }

        public void ChangeState(int id, StateTransitionMode mode = StateTransitionMode.Deferred)
        {
            if (!Running)
            {
                return;
            }

            StateBase newState = _registry.Get(id);
            Schedule(newState, mode);
        }

        public void ChangeState<TPayload>(int id, TPayload payload, StateTransitionMode mode = StateTransitionMode.Deferred)
        {
            if (!Running)
            {
                return;
            }

            StateBase state = _registry.Get(id);
            if (state is not IStatePayload<TPayload> statePayload)
            {
                throw new InvalidOperationException(
                    $"State '{state.GetType().Name}' does not accept a payload of type '{typeof(TPayload).Name}'");
            }

            statePayload.SetPayload(payload);
            Schedule(state, mode);
        }

        public void Update()
        {
            if (!Running)
            {
                return;
            }

            FlushPending();

            if (Current == null)
            {
                return;
            }

            if (Current.TryGetTransition(out int target))
            {
                ChangeState(target);
                return;
            }

            Current.Update();
        }

        public void FixedUpdate()
        {
            if (!Running)
            {
                return;
            }

            if (Current == null)
            {
                return;
            }

            if (Current.TryGetFixedTransition(out int target))
            {
                ChangeState(target);
                return;
            }

            Current.FixedUpdate();
        }

        private void Schedule(StateBase state, StateTransitionMode mode)
        {
            if (Pending != null && Pending != state)
            {
                GameLog.Warning(
                    this,
                    $"Transition to '{Pending.GetType().Name}' was replaced by '{state.GetType().Name}' before it took effect.");
            }

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

            while (Pending != null)
            {
                if (guard++ >= TransitionGuardThreshold)
                {
                    GameLog.Warning(this, $"Transition guard ({TransitionGuardThreshold}) hit while leaving '{Current?.GetType().Name}'");

                    Pending = null;
                    break;
                }

                StateBase next = Pending;
                Pending = null;

                Current?.Exit();
                Current = next;
                Current.Enter();

                StateEntered?.Invoke(Current);
            }

            _isFlushing = false;
        }
    }
}
