using LayerZero.Characters.Common.States;

namespace LayerZero.Characters.Player.States
{
    /// <summary>
    /// Terminal player state. Hook the game-over flow in here (respawn prompt, checkpoint reload)
    /// - the state machine guarantees nothing else runs afterwards.
    /// </summary>
    public sealed class PlayerDeadState : DeadStateBase<PlayerController>
    {
        public PlayerDeadState(PlayerController owner)
            : base(owner)
        {
        }
    }
}
