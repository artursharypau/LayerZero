using Characters.Common.Animation;
using Characters.Common.States;
using Characters.Player.Animation;
using Characters.Player.Input;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAttackState : AttackState<PlayerController>
    {
        private readonly int _startIndex;
        private readonly int _lastIndex;
        private readonly CountdownTimer _velocityTimer;

        private int _currIndex;
        private float _finishedTime;
        private bool _nextAttackQueued;

        public PlayerAttackState(PlayerController controller)
            : base(controller, AnimatorHashProvider.Attack)
        {
            _startIndex = 0;
            _lastIndex = Controller.AttacksCount - 1;
            _velocityTimer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _nextAttackQueued = false;

            SetIndex();
            ApplyVelocity();

            Controller.Combat.SetActiveAttackDefinition(Controller.AttackDefinitions[_currIndex]);
        }

        public override void Update()
        {
            base.Update();

            if (Controller.Input.WasPerformed(PlayerInputAction.Attack))
            {
                _nextAttackQueued = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _velocityTimer.Tick(Time.fixedDeltaTime);
            if (_velocityTimer.IsExpired)
            {
                Controller.Movement.SetVelocityX(0f);
            }
        }

        protected override void OnAttackFinished()
        {
            ++_currIndex;
            _finishedTime = Time.time;

            if (!_nextAttackQueued || _currIndex > _lastIndex)
            {
                Controller.ChangeState(Controller.Input.Move.x != 0f ? PlayerStateId.Move : PlayerStateId.Idle);
            }
            else
            {
                Controller.ChangeState(PlayerStateId.Attack);
            }
        }

        private void SetIndex()
        {
            bool comboExpired = Time.time - _finishedTime > Controller.AttackResetTime;
            if (comboExpired || _currIndex > _lastIndex)
            {
                _currIndex = _startIndex;
            }
            else
            {
                _currIndex = _currIndex > _lastIndex ? _startIndex : _currIndex;
            }

            Anim.SetInteger(PlayerAnimatorHashProvider.AttackIndex, _currIndex);
        }

        private void ApplyVelocity()
        {
            _velocityTimer.Start(Controller.AttackVelocityDuration);

            Vector2 velocity = Controller.AttackVelocities[_currIndex];
            float velocityX = Controller.Input.Move.x != 0f
                ? Controller.Input.Move.x * velocity.x
                : velocity.x * Controller.Movement.FacingDirection;

            Controller.Movement.SetVelocity(velocityX, velocity.y);
        }
    }
}
