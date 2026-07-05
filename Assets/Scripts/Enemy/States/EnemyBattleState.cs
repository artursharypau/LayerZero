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
            : base(EnemyAnimationIdProvider.Battle, fsm, controller)
        {
        }

        public override void Enter()
        {
            _initialMoveAnimMultiplier = Anim.GetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier);
            _playerTransform = Controller.CheckForPlayer().transform;
            if (!_playerTransform)
            {
                FSM.ChangeState(Controller.IdleState);
            }

            Anim.SetFloat(EnemyAnimationIdProvider.BattleMoveAnimMultiplier, Controller.BattleMoveAnimMultiplier);

            base.Enter();
        }

        public override void Update()
        {
            Anim.SetFloat(AnimationIdProvider.VelocityX, Controller.RB.linearVelocityX);

            float currentTime = Time.time;
            if (Controller.IsPlayerDetected)
            {
                _lastPlayerDetectedTime = currentTime;
            }

            if (currentTime >= _lastPlayerDetectedTime + Controller.InBattleChaseDuration)
            {
                FSM.ChangeState(Controller.IdleState);
            }
            // else if (Controller.AttackDistance >= GetPlayerAbsDistance())
            // {
            //     FSM.ChangeState(Controller.AttackState);
            // }
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
            _playerTransform = null;

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
