using Core.Animation;
using StateMachine = Core.StateMachine.StateMachine;

namespace Characters.Enemy.States
{
    public class EnemyChaseState : EnemyState
    {
        private float _initialMoveAnimMultiplier;

        public EnemyChaseState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, EnemyAnimatorHashProvider.Chase)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _initialMoveAnimMultiplier = Anim.GetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier);

            Anim.SetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier, Controller.ChaseMoveAnimMultiplier);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (!Controller.Target.HasCurrent)
            {
                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            if (Controller.ShouldAttack())
            {
                FSM.ChangeState(Controller.AttackState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimatorHashProvider.VelocityX, Controller.RB.linearVelocityX);

            if (Controller.Target.IsBehind)
            {
                Controller.Flip();
            }

            if (Controller.IsGrounded && !Controller.IsWalled)
            {
                Controller.SetVelocity(
                    Controller.MoveSpeed * Controller.ChaseMoveSpeedMultiplier * Controller.Target.Direction,
                    Controller.RB.linearVelocityY);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Anim.SetFloat(EnemyAnimatorHashProvider.ChaseMoveAnimMultiplier, _initialMoveAnimMultiplier);
        }
    }
}
