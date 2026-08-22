namespace LayerZero.Gameplay.Characters.Player.States
{
    public sealed class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(PlayerController owner)
            : base(owner)
        {
            On(() => Input.Move.x == 0f, PlayerStateId.Idle);

            On(IsPushingIntoWall, PlayerStateId.Idle);
        }

        public override int Id => PlayerStateId.Move;

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
