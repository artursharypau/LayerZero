using Common;
using UnityEngine;

namespace Player.States
{
    public class PlayerBasicAttackState : PlayerState
    {
        private const int StartIndex = 0;
        private static readonly int EndIndex = PlayerAnimationIdProvider.BasicAttacks.Length - 1;

        private int _currIndex;
        private float _finishedTime;
        private float _velocityTimer;
        private bool _nextAttackQueued;

        public PlayerBasicAttackState(StateMachine fsm, PlayerController controller)
            : base(PlayerAnimationIdProvider.BasicAttack, fsm, controller)
        {
        }

        public override void Enter()
        {
            Controller.AnimTriggers.AttackFinished += OnFinished;

            _nextAttackQueued = false;

            SetIndex();
            PlayAnimationByIndex();
            ApplyVelocity();

            base.Enter();
        }

        public override void Update()
        {
            if (_velocityTimer <= 0f)
            {
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            }

            if (Controller.InputActions.BasicAttack.WasPerformedThisFrame())
            {
                _nextAttackQueued = true;
            }

            _velocityTimer -= Time.deltaTime;

            base.Update();
        }

        public override void Exit()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            base.Exit();
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
        }

        private void PlayAnimationByIndex()
        {
            Anim.Play(PlayerAnimationIdProvider.BasicAttacks[_currIndex], 0, 0f);
        }

        private void ApplyVelocity()
        {
            _velocityTimer = Controller.AttackVelocityDuration;

            Vector2 velocity = Controller.AttackVelocity[_currIndex];
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
                FSM.ChangeState(Controller.BasicAttackState);
            }
        }
    }
}
