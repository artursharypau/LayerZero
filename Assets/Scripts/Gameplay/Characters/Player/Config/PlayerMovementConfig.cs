using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    internal sealed class PlayerMovementConfig
    {
        [SerializeField] [Min(0f)] private float _moveSpeed = 15f;
        [SerializeField] [Range(0f, 1f)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0f, 1f)] private float _wallSlideMultiplier = 0.8f;

        public float MoveSpeed => _moveSpeed;
        public float InAirMoveMultiplier => _inAirMoveMultiplier;
        public float WallSlideMultiplier => _wallSlideMultiplier;
    }
}
