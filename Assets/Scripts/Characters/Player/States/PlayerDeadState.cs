using LayerZero.Characters.Common.States;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerDeadState : DeadStateBase<PlayerController>
    {
        public PlayerDeadState(PlayerController owner)
            : base(owner)
        {
        }
    }
}
