using UnityEngine;

namespace Core.StateMachine
{
    public class StateMachine
    {
        private const int _guardThreshold = 8;

        public State Pending { get; private set; }
        public State Current { get; private set; }

        public void Start(State initialState)
        {
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

        private void ApplyPendingTransition()
        {
            int guard = 0;

            // Loop instead of a single check: Enter() below can itself call ChangeState(), which sets Pending again.
            // We must keep draining it so Current never ends up being a state that already requested its own replacement.
            while (Pending != null)
            {
                if (guard++ >= _guardThreshold)
                {
                    Debug.unityLogger.LogWarning(
                        $"{nameof(StateMachine)}.{nameof(ApplyPendingTransition)}",
                        $"Guard threshold ({_guardThreshold}) reached while transitioning away from '{Current?.GetType().Name}'. Possible state transition loop — check TryTransition()/Enter() logic.");

                    break;
                }

                Current.Exit();
                Current = Pending;
                Pending = null;
                Current.Enter();
            }
        }
    }
}
