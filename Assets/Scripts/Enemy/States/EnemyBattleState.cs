using Common;
using Common.Animations;
using UnityEngine;

namespace Enemy.States
{
    public class EnemyBattleState : EnemyState
    {
        private float _initialMoveAnimMultiplier;
        private float _lastPlayerDetectedTime;
        private Transform _playerTransform;

        public EnemyBattleState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, EnemyAnimationIdProvider.Battle)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _initialMoveAnimMultiplier = Anim.GetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier);
            _lastPlayerDetectedTime = Time.time;

            if (!_playerTransform)
            {
                _playerTransform = Controller.CheckForPlayer().transform;
            }

            Anim.SetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier, Controller.BattleMoveAnimMultiplier);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Time.time >= _lastPlayerDetectedTime + Controller.InBattleChaseDuration)
            {
                _playerTransform = null;
                FSM.ChangeState(Controller.IdleState);

                return true;
            }

            if (Controller.IsPlayerDetected && Controller.AttackDistance >= GetPlayerAbsDistance())
            {
                FSM.ChangeState(Controller.AttackState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimationHashProvider.VelocityX, Controller.RB.linearVelocityX);

            if (Controller.IsPlayerDetected)
            {
                _lastPlayerDetectedTime = Time.time;
            }

            Controller.SetVelocity(
                Controller.MoveSpeed * Controller.BattleMoveSpeedMultiplier * GetPlayerDirection(),
                Controller.RB.linearVelocityY);
        }

        public override void Exit()
        {
            base.Exit();

            Anim.SetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier, _initialMoveAnimMultiplier);
        }

        private float GetPlayerAbsDistance()
        {
            return Mathf.Abs(_playerTransform.position.x - Controller.transform.position.x);
        }

        private int GetPlayerDirection()
        {
            return _playerTransform.position.x > Controller.transform.position.x ? 1 : -1;
        }
    }
}
