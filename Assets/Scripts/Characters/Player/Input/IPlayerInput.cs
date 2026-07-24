using UnityEngine;

namespace Characters.Player.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }

        bool WasPerformed(PlayerInputAction action);
        void Consume(PlayerInputAction action);
    }
}
