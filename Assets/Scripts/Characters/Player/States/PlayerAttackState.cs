using Characters.Common;
using Core.Animation;
using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAttackState : AttackState<PlayerController>
    {
        private const int StartIndex = 0;
        private const int EndIndex = 2;

        private readonly CountdownTimer _velocityTimer;

        private int _currIndex;
        private float _finishedTime;
        private bool _nextAttackQueued;

        public PlayerAttackState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, AnimatorHashProvider.Attack)
        {
            _velocityTimer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _nextAttackQueued = false;

            SetIndex();
            ApplyVelocity();
        }

        public override void Update()
        {
            base.Update();

            _velocityTimer.Tick(Time.deltaTime);
            if (_velocityTimer.IsExpired)
            {
                Controller.SetHorizontalVelocity(0f);
            }

            if (Controller.InputHandler.WasAttackPerformed())
            {
                _nextAttackQueued = true;
            }
        }

        protected override void OnAttackFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnAttackFinished;

            ++_currIndex;
            _finishedTime = Time.time;

            if (!_nextAttackQueued || _currIndex > EndIndex)
            {
                FSM.ChangeState(Controller.InputHandler.Move.x != 0f ? Controller.MoveState : Controller.IdleState);
            }
            else
            {
                FSM.ChangeState(Controller.AttackState);
            }
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
            _velocityTimer.Start(Controller.AttackVelocityDuration);

            Vector2 velocity = Controller.AttackVelocities[_currIndex];
            float velocityX = Controller.InputHandler.Move.x != 0f
                ? Controller.InputHandler.Move.x * velocity.x
                : velocity.x * Controller.FacingDirection;

            Controller.SetVelocity(velocityX, velocity.y);
        }
    }
}
