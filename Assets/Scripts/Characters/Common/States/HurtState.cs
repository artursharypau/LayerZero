using Characters.Common.Animation;
using Core.StateMachine;
using Core.Utils;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common.States
{
    public abstract class HurtState<TController> : AnimatedState<TController>, IStateArg<DamageImpactInfo>
        where TController : CharacterController2D
    {
        private const float MinKnockbackLockDuration = 0.1f;

        private readonly CountdownTimer _timer = new();

        private DamageImpactInfo _damageImpact;

        protected HurtState(TController controller, int animHash, AnimatorParameterType type = AnimatorParameterType.None)
            : base(controller, animHash, type)
        {
        }

        public void Prepare(DamageImpactInfo arg)
        {
            _damageImpact = arg;
        }

        public override void Enter()
        {
            base.Enter();

            DamageImpactInfo damageImpact = _damageImpact;
            Controller.Movement.SetVelocity(damageImpact.Knockback.x, damageImpact.Knockback.y);

            float duration = damageImpact.StunDuration;
            if (duration <= 0f && damageImpact.Knockback != Vector2.zero)
            {
                duration = MinKnockbackLockDuration;
            }

            _timer.Start(duration);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
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

        public override void Update()
        {
            base.Update();

            _timer.Tick(Time.deltaTime);
        }

        protected abstract void OnHurtFinished();
    }
}
