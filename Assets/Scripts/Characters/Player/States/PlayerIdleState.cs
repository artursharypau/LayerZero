using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Idle)
        {
        }

        public override int Id => PlayerStateId.Idle;

        public override void Enter()
        {
            base.Enter();

            Movement.SetVelocityX(0f);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Input.Move.x != 0f && !IsPushingIntoWall())
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Move);
                return true;
            }

            return false;
        }
    }
}
