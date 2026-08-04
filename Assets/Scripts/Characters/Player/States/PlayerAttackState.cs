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
    public sealed class PlayerAttackState : AttackStateBase<PlayerController>
    {
        private readonly CountdownTimer _movementLockTimer = new();

        private int _stepIndex;
        private float _lastFinishedTime = float.NegativeInfinity;
        private bool _isNextQueued;

        public PlayerAttackState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Attack)
        {
        }

        public override int Id => PlayerStateId.Attack;

        private PlayerAttackSettings Settings => Owner.Config.Attack;

        protected override void OnPrepareAttack()
        {
            _isNextQueued = false;

            AdvanceComboCursor();
            Animator.SetInt(PlayerAnimatorParameters.AttackIndex, _stepIndex);
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

            _movementLockTimer.Tick(Time.fixedDeltaTime);
            if (_movementLockTimer.IsExpired)
            {
                Movement.SetVelocityX(0f);
            }
        }

        protected override AttackDefinition ResolveAttackDefinition()
        {
            return Settings.GetStep(_stepIndex)?.Attack;
        }

        protected override void OnAttackFinished()
        {
            ++_stepIndex;
            _lastFinishedTime = Time.time;

            if (_isNextQueued && _stepIndex < Settings.ComboLength)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Attack);
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

            _movementLockTimer.Start(Settings.VelocityDuration);

            float velocityX = Owner.Input.Move.x != 0f
                ? Owner.Input.Move.x * step.Velocity.x
                : step.Velocity.x * Movement.FacingDirection;

            Movement.SetVelocity(velocityX, step.Velocity.y);
        }

        private void ChangeToLocomotion()
        {
            if (Owner.Input.Move.x != 0f)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Move);
            }
            else
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Idle);
            }
        }
    }
}
