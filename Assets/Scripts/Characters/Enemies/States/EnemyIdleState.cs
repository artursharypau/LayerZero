using LayerZero.Characters.Common.Animation;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyIdleState : EnemyGroundedState
    {
        private readonly CountdownTimer _timer = new();

        public EnemyIdleState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Idle)
        {
        }

        public override int Id => EnemyStateId.Idle;

        public override void Enter()
        {
            base.Enter();

            _timer.Start(Config.Movement.IdleDuration);
            Movement.SetVelocityX(0f);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_timer.IsExpired)
            {
                ChangeTo(EnemyStateId.Patrol);
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
