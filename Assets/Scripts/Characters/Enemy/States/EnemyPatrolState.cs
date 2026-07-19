using Core.StateMachine;

namespace Characters.Enemy.States
{
    public class EnemyPatrolState : EnemyGroundedState
    {
        public EnemyPatrolState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, EnemyAnimatorHashProvider.Patrol)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                Controller.Flip();
            }

            Anim.SetFloat(EnemyAnimatorHashProvider.MoveAnimMultiplier, Controller.MoveAnimMultiplier);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Controller.SetHorizontalVelocity(Controller.MoveSpeed * Controller.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetHorizontalVelocity(0f);
        }
    }
}
