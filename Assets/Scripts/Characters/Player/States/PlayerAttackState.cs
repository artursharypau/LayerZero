using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Animation;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;
using LayerZero.Combat.Attacks;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    /// <summary>
    /// Ground combo. The state owns the combo cursor; each swing's lunge and damage come from
    /// one <see cref="AttackComboStep" />, so combo length is pure data.
    /// </summary>
    public sealed class PlayerAttackState : AttackStateBase<PlayerController>
    {
        private readonly CountdownTimer _lungeTimer = new();

        private int _stepIndex;
        private float _lastFinishedTime = float.NegativeInfinity;
        private bool _isNextQueued;

        public PlayerAttackState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Attack)
        {
        }

        private PlayerAttackSettings Settings => Owner.Config.Attack;

        protected override void OnPrepareAttack()
        {
            _isNextQueued = false;

            AdvanceComboCursor();
            Animation.SetInt(PlayerAnimatorParameters.AttackIndex, _stepIndex);
        }

        public override void Enter()
        {
            base.Enter();

            ApplyLunge();
        }

        public override void Update()
        {
            base.Update();

            if (Owner.Input.WasPerformed(PlayerInputAction.Attack))
            {
                _isNextQueued = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _lungeTimer.Tick(Time.fixedDeltaTime);
            if (_lungeTimer.IsExpired)
            {
                Movement.SetVelocityX(0f);
            }
        }

        protected override AttackDefinition ResolveAttack()
        {
            return Settings.GetStep(_stepIndex)?.Attack;
        }

        protected override void OnAttackFinished()
        {
            ++_stepIndex;
            _lastFinishedTime = Time.time;

            if (_isNextQueued && _stepIndex < Settings.ComboLength)
            {
                ChangeTo<PlayerAttackState>();
                return;
            }

            ChangeToLocomotion();
        }

        private void AdvanceComboCursor()
        {
            bool comboExpired = Time.time - _lastFinishedTime > Settings.ComboResetDelay;
            if (comboExpired || _stepIndex >= Settings.ComboLength)
            {
                _stepIndex = 0;
            }
        }

        private void ApplyLunge()
        {
            AttackComboStep step = Settings.GetStep(_stepIndex);
            if (step == null)
            {
                return;
            }

            _lungeTimer.Start(Settings.VelocityDuration);

            // Lunge follows the stick when the player is steering, otherwise the current facing.
            float velocityX = Owner.Input.Move.x != 0f
                ? Owner.Input.Move.x * step.Velocity.x
                : step.Velocity.x * Movement.FacingDirection;

            Movement.SetVelocity(velocityX, step.Velocity.y);
        }

        private void ChangeToLocomotion()
        {
            if (Owner.Input.Move.x != 0f)
            {
                ChangeTo<PlayerMoveState>();
            }
            else
            {
                ChangeTo<PlayerIdleState>();
            }
        }
    }
}
