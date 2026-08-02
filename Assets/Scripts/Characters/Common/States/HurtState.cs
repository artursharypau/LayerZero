using Characters.Common.Animation;
using Core.Utils;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common.States
{
    public abstract class HurtState<TController> : AnimatedState<TController>
        where TController : CharacterController2D
    {
        private const float MinKnockbackLockDuration = 0.12f;

        private readonly CountdownTimer _timer = new();

        protected HurtState(TController controller, int animHash, AnimatorParameterType type = AnimatorParameterType.None)
            : base(controller, animHash, type)
        {
        }

        public override void Enter()
        {
            base.Enter();

            DamageImpactInfo impact = DamageImpactInfo.None;
            Controller.Movement.SetVelocity(impact.Knockback.x, impact.Knockback.y);

            float duration = impact.StunDuration;
            if (duration <= 0f && impact.Knockback != Vector2.zero)
            {
                duration = MinKnockbackLockDuration;
            }

            _timer.Start(duration);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (_timer.IsExpired)
            {
                OnHurtFinished();
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _timer.Tick(Time.fixedDeltaTime);
        }

        protected abstract void OnHurtFinished();
    }
}
