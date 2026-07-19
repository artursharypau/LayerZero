using Core.Animation;
using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Enemy.States
{
    public class EnemyIdleState : EnemyGroundedState
    {
        private readonly CountdownTimer _timer;

        public EnemyIdleState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, AnimatorHashProvider.Idle)
        {
            _timer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _timer.Start(Controller.IdleDuration);
            Controller.SetHorizontalVelocity(0f);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (!_timer.IsRunning)
            {
                FSM.ChangeState(Controller.PatrolState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _timer.Tick(Time.deltaTime);
        }
    }
}
