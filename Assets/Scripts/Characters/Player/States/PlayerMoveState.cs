using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Move)
        {
        }

        public override int Id => PlayerStateId.Move;

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Input.Move.x == 0f)
            {
                ChangeTo(PlayerStateId.Idle);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (IsPushingIntoWall())
            {
                ChangeTo(PlayerStateId.Idle);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Movement.SetVelocityX(Config.Movement.MoveSpeed * Input.Move.x, true);
        }

        public override void Exit()
        {
            base.Exit();

            Movement.SetVelocityX(0f);
        }
    }
}
