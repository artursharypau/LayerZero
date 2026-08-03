using UnityEngine;

namespace LayerZero.Characters.Player.Input
{
    /// <summary>
    /// Intent, not devices. States and abilities depend on this, never on the Input System,
    /// which keeps them testable and lets buffering live in one place.
    /// </summary>
    public interface IPlayerInput
    {
        Vector2 Move { get; }

        bool WasPerformed(PlayerInputAction action);
        void Consume(PlayerInputAction action);
    }
}
