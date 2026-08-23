namespace LayerZero.Gameplay.Characters.Player.States
{
    internal sealed class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(PlayerController owner)
            : base(owner)
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
