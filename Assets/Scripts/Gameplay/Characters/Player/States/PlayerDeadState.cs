namespace LayerZero.Gameplay.Characters.Player.States
{
    internal sealed class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerController owner)
            : base(owner)
        {
        }

        public override int Id => PlayerStateId.Dead;
    }
}
