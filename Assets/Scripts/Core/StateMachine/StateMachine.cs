using UnityEngine;

namespace Core.StateMachine
{
    public class StateMachine
    {
        private const int GuardThreshold = 8;

        public State Pending { get; private set; }
        public State Current { get; private set; }

        public void Start(State initialState)
        {
            if (initialState == null)
            {
                return;
            }

            Current = initialState;
            Current.Enter();
        }

        public void ChangeState(State newState)
        {
            if (newState == null)
            {
                return;
            }

            Pending = newState;
        }

        public void ChangeState<TArg>(State newState, TArg arg)
        {
            if (newState == null)
            {
                return;
            }

            if (newState is IStateArg<TArg> stateArg)
            {
                stateArg.Prepare(arg);
            }

            ChangeState(newState);
        }

        public void Update()
        {
            ApplyPendingTransition();

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
            ApplyPendingTransition();

            if (Current == null)
            {
                return;
            }

            if (!Current.TryFixedTransition())
            {
                Current.FixedUpdate();
            }
        }

        private void ApplyPendingTransition()
        {
            int guard = 0;

            // Loop instead of a single check: Enter() below can itself call ChangeState(), which sets Pending again.
            // We must keep draining it so Current never ends up being a state that already requested its own replacement.
            while (Pending != null)
            {
                if (guard++ >= GuardThreshold)
                {
                    Debug.unityLogger.LogWarning(
                        $"{nameof(StateMachine)}.{nameof(ApplyPendingTransition)}",
                        $"Guard threshold ({GuardThreshold}) reached while transitioning away from '{Current?.GetType().Name}'. Possible state transition loop — check TryTransition()/Enter() logic.");

                    break;
                }

                Current?.Exit();
                Current = Pending;
                Pending = null;
                Current.Enter();
            }
        }
    }
}
