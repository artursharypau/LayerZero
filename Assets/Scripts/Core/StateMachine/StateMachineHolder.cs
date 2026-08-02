using System;
using System.Collections.Generic;

namespace Core.StateMachine
{
    public class StateMachineHolder
    {
        private readonly StateMachine _stateMachine = new();
        private readonly Dictionary<int, State> _states = new();

        public void Register(State state)
        {
            _states[state.Id] = state;
        }

        public void Start(int initialId)
        {
            State state = Resolve(initialId);
            _stateMachine.Start(state);
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void ChangeState(int id)
        {
            State state = Resolve(id);
            _stateMachine.ChangeState(state);
        }

        public void ChangeState<TArg>(int id, TArg arg)
        {
            State state = Resolve(id);
            if (state is not IStateArg<TArg> stateArg)
            {
                throw new InvalidOperationException(
                    $"State '{state.GetType().Name}' does not accept argument of type '{typeof(TArg).Name}'");
            }

            stateArg.Prepare(arg);
            _stateMachine.ChangeState(state);
        }

        private State Resolve(int id)
        {
            if (_states.TryGetValue(id, out State state))
            {
                return state;
            }

            throw new KeyNotFoundException($"State with id '{id}' was not found");
        }
    }
}
