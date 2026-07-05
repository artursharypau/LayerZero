using Common;
using Common.Animations;
using UnityEngine;

namespace Enemy.States
{
    public class EnemyIdleState : EnemyGroundedState
    {
        private float _idleTimer;

        public EnemyIdleState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, AnimationHashProvider.Idle)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _idleTimer = Controller.IdleDuration;
            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_idleTimer <= 0f)
            {
                FSM.ChangeState(Controller.MoveState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _idleTimer -= Time.deltaTime;
        }
    }
}
