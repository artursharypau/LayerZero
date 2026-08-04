using LayerZero.Characters.Common.Animation;
using LayerZero.Combat.Damage;
using LayerZero.Core.StateMachine;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Common.States
{
    public abstract class HurtStateBase<TCharacter> : CharacterState<TCharacter>, IStatePayload<DamageImpactInfo>
        where TCharacter : Character2D
    {
        private const float MinKnockbackLockDuration = 0.1f;

        private readonly CountdownTimer _timer = new();

        private DamageImpactInfo _impact;

        protected HurtStateBase(TCharacter owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public void SetPayload(DamageImpactInfo payload)
        {
            _impact = payload;
        }

        public override void Enter()
        {
            base.Enter();

            DamageImpactInfo impact = _impact;
            _impact = DamageImpactInfo.None;

            Movement.SetVelocity(impact.Knockback.x, impact.Knockback.y);

            float duration = impact.StunDuration;
            if (duration <= 0f && impact.Knockback != Vector2.zero)
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
