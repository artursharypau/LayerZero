using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Idle)
        {
            On(() => Input.Move.x != 0f && !IsPushingIntoWall(), PlayerStateId.Move);
        }

        public override int Id => PlayerStateId.Idle;

        public override void Enter()
        {
            base.Enter();

            Movement.SetVelocityX(0f);
        }
    }
}
