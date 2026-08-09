using UnityEngine;

namespace LayerZero.Characters.Player.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }

        void Enable();
        void Disable();
        bool WasPerformed(PlayerInputAction action);
        void Consume(PlayerInputAction action);
    }
}
