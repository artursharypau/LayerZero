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
            _idleTimer = Controller.IdleDuration;
            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);

            base.Enter();
        }

        public override void Update()
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
            {
                FSM.ChangeState(Controller.MoveState);
            }

            base.Update();
        }
    }
}
