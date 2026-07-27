using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Enemy.States
{
    public class EnemyChaseState : EnemyState
    {
        private float _initialMoveAnimMultiplier;

        public EnemyChaseState(EnemyController controller)
            : base(controller, EnemyAnimatorHashProvider.Chase)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _initialMoveAnimMultiplier = Anim.GetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier);
            Controller.TargetDetector.TargetLost += OnTargetLost;
            Anim.SetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier, Controller.ChaseMoveAnimMultiplier);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.ShouldAttack())
            {
                Controller.ChangeState(StateId.Attack);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimatorHashProvider.VelocityX, Controller.VelocityX);

            if (Controller.TargetDetector.IsBehind)
            {
                Controller.Flip();
            }

            if (Controller.IsGrounded && !Controller.IsWalled)
            {
                Controller.SetVelocityX(
                    Controller.MoveSpeed * Controller.ChaseMoveSpeedMultiplier * Controller.TargetDetector.Direction);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Controller.TargetDetector.TargetLost -= OnTargetLost;
            Anim.SetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier, _initialMoveAnimMultiplier);
        }

        private void OnTargetLost()
        {
            Controller.TargetDetector.TargetLost -= OnTargetLost;
            Controller.ChangeState(StateId.Idle);
        }
    }
}
