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

            Controller.SetVelocity(Controller.MoveSpeed * Controller.FacingDirection, Controller.RB.linearVelocityY);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
        }
    }
}
