using Common;
using Common.Animations;
using UnityEngine;

namespace Enemy.States
{
    public class EnemyBattleState : EnemyState
    {
        private float _initialMoveAnimMultiplier;
        private Transform _playerTransform;
        private float _lastPlayerDetectedTime;

        public EnemyBattleState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, EnemyAnimationIdProvider.Battle)
        {
        }

        public override void Enter()
        {
            _initialMoveAnimMultiplier = Anim.GetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier);

            if (!_playerTransform)
            {
                _playerTransform = Controller.CheckForPlayer().transform;
            }

            Anim.SetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier, Controller.BattleMoveAnimMultiplier);

            base.Enter();
        }

        public override void Update()
        {
            Anim.SetFloat(AnimationHashProvider.VelocityX, Controller.RB.linearVelocityX);

            bool isPlayerDetected = Controller.IsPlayerDetected;
            float currentTime = Time.time;

            if (isPlayerDetected)
            {
                _lastPlayerDetectedTime = currentTime;
            }

            if (currentTime >= _lastPlayerDetectedTime + Controller.InBattleChaseDuration)
            {
                _playerTransform = null;
                FSM.ChangeState(Controller.IdleState);
            }
            else if (isPlayerDetected && Controller.AttackDistance >= GetPlayerAbsDistance())
            {
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
                FSM.ChangeState(Controller.AttackState);
            }
            else
            {
                Controller.SetVelocity(
                    Controller.MoveSpeed * Controller.BattleMoveSpeedMultiplier * GetPlayerDirection(),
                    Controller.RB.linearVelocityY);
            }

            base.Update();
        }

        public override void Exit()
        {
            Anim.SetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier, _initialMoveAnimMultiplier);

            base.Exit();
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
