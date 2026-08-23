using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    internal sealed class PlayerInputConfig
    {
        [SerializeField] [Min(0f)] private float _jumpBufferDuration = 0.2f;
        [SerializeField] [Min(0f)] private float _dashBufferDuration = 0.1f;

        public float JumpBufferDuration => _jumpBufferDuration;
        public float DashBufferDuration => _dashBufferDuration;
    }
}
