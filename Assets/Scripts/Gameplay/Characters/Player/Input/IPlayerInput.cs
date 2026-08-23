using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Input
{
    internal interface IPlayerInput
    {
        Vector2 Move { get; }

        void Enable();
        void Disable();
        bool WasPerformed(PlayerInputAction action);
        void Consume(PlayerInputAction action);
    }
}
