using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;
using LayerZero.Combat.Attacks;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerAttackState : PlayerState
    {
        private readonly AttackBehaviour _attack;

        private Countdown _lungeTimer;
        private int _comboStepIndex;
        private float _lastFinishedTime = -1f;
        private bool _isNextAttackQueued;

        public PlayerAttackState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Attack)
        {
            _attack = new AttackBehaviour(owner, ResolveAttack);

            On(() => _attack.IsFinished, ResolveNextState);
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Dash), PlayerStateId.Dash);
        }

        public override int Id => PlayerStateId.Attack;

        public override void Enter()
        {
            base.Enter();

            _isNextAttackQueued = false;

            TryResetComboStepIndex();
            Animator.SetInt(PlayerAnimatorParameters.AttackIndex, _comboStepIndex);

            _attack.Begin();
            ApplyLunge();
        }

        public override void Update()
        {
            base.Update();

            if (Input.WasPerformed(PlayerInputAction.Attack))
            {
                _isNextAttackQueued = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_lungeTimer.IsExpired)
            {
                Movement.SetVelocityX(0f);
            }
        }

        public override void Exit()
        {
            base.Exit();

            if (_attack.IsFinished)
            {
                ++_comboStepIndex;
                _lastFinishedTime = Time.time;
            }

            _attack.End();
        }

        private AttackDefinition ResolveAttack()
        {
            return Config.Attack.GetComboStep(_comboStepIndex)?.Attack;
        }

        private int ResolveNextState()
        {
            bool hasNext = _isNextAttackQueued && _comboStepIndex + 1 < Config.Attack.ComboLength;
            return hasNext ? PlayerStateId.Attack : ResolveLocomotionState();
        }

        private void TryResetComboStepIndex()
        {
            bool comboExpired = Time.time - _lastFinishedTime > Config.Attack.ComboResetDelay;
            if (comboExpired || _comboStepIndex >= Config.Attack.ComboLength)
            {
                _comboStepIndex = 0;
            }
        }

        private void ApplyLunge()
        {
            AttackComboStep step = Config.Attack.GetComboStep(_comboStepIndex);
            if (step == null)
            {
                return;
            }

            _lungeTimer.Start(Config.Attack.VelocityDuration);

            float velocityX = Input.Move.x != 0f
                ? Input.Move.x * step.Velocity.x
                : step.Velocity.x * Movement.FacingDirection;

            Movement.SetVelocity(velocityX, step.Velocity.y);
        }
    }
}
