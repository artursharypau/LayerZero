using Characters.Common.Animation;
using Characters.Common.States;
using Core.Utils;
using UnityEngine;

namespace Characters.Enemy.States
{
    public class EnemyIdleState : EnemyGroundedState
    {
        private readonly CountdownTimer _timer;

        public EnemyIdleState(EnemyController controller)
            : base(controller, AnimatorHashProvider.Idle)
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

            if (_timer.IsExpired)
            {
                Controller.ChangeState(StateId.Patrol);
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
