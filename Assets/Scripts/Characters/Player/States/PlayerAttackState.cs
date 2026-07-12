using Core.Animation;
using Core.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAttackState : PlayerState
    {
        private const int StartIndex = 0;
        private const int EndIndex = 2;

        private int _currIndex;
        private float _finishedTime;
        private float _velocityTimer;
        private bool _nextAttackQueued;

        public PlayerAttackState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, AnimatorHashProvider.Attack, AnimatorParameterType.Trigger)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.AnimTriggers.AttackFinished += OnFinished;

            _nextAttackQueued = false;

            SetIndex();
            ApplyVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (_velocityTimer <= 0f)
            {
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            }

            if (Controller.InputActions.Attack.WasPerformedThisFrame())
            {
                _nextAttackQueued = true;
            }

            _velocityTimer -= Time.deltaTime;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AnimTriggers.AttackFinished -= OnFinished;
        }

        private void SetIndex()
        {
            if (Time.time - _finishedTime > Controller.AttackResetTime)
            {
                _currIndex = StartIndex;
            }
            else
            {
                _currIndex = _currIndex > EndIndex ? StartIndex : _currIndex;
            }

            Anim.SetInteger(PlayerAnimatorHashProvider.AttackIndex, _currIndex);
        }

        private void ApplyVelocity()
        {
            _velocityTimer = Controller.AttackVelocityDuration;

            Vector2 velocity = Controller.AttackVelocities[_currIndex];
            float velocityX = Controller.MoveInput.x != 0f
                ? Controller.MoveInput.x * velocity.x
                : velocity.x * Controller.FacingDirection;

            Controller.SetVelocity(velocityX, velocity.y);
        }

        private void OnFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            ++_currIndex;
            _finishedTime = Time.time;

            if (!_nextAttackQueued || _currIndex > EndIndex)
            {
                FSM.ChangeState(Controller.MoveInput.x != 0f ? Controller.MoveState : Controller.IdleState);
            }
            else
            {
                FSM.ChangeState(Controller.AttackState);
            }
        }
    }
}
