using LayerZero.Characters.Common.Animation;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>Post-attack breather. Keeps ranged enemies from firing every frame.</summary>
    public sealed class EnemyRecoverState : EnemyState
    {
        private readonly CountdownTimer _timer = new();
        private readonly float _duration;

        public EnemyRecoverState(EnemyController owner, float duration)
            : base(owner, CommonAnimatorParameters.Idle)
        {
            _duration = duration;
        }

        public override void Enter()
        {
            base.Enter();

            _timer.Start(_duration);
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
                ChangeTo<EnemyChaseState>();
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
