using System;
using LayerZero.Gameplay.Combat.Elements;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Input
{
    internal interface IPlayerInput
    {
        event Action<ElementKind> ElementSelected;

        Vector2 Move { get; }

        void Enable();
        void Disable();
        bool WasPerformed(PlayerInputAction action);
        void Consume(PlayerInputAction action);
    }
}
