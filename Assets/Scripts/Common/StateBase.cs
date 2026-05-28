using UnityEngine;

namespace Common
{
    public abstract class StateBase
    {
        private readonly int _animEntryId;

        protected StateMachine FSM { get; }
        protected Animator Anim { get; }

        protected StateBase(int animEntryId, StateMachine fsm, Animator anim)
        {
            _animEntryId = animEntryId;

            FSM = fsm;
            Anim = anim;
        }

        public virtual void Enter()
        {
            Anim.SetBool(_animEntryId, true);
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            Anim.SetBool(_animEntryId, false);
        }
    }
}
