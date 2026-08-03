using System;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [Serializable]
    public sealed class JumpAbilitySettings
    {
        [SerializeField] [Min(1)] private int _charges = 2;
        [SerializeField] [Min(0f)] private float _force = 13f;

        [Header("Wall jump")]
        [SerializeField] private Vector2 _wallJumpForce = new(6f, 12f);
        [SerializeField] [Min(0f)] private float _wallJumpMoveLockDuration = 0.2f;

        public int Charges => _charges;
        public float Force => _force;
        public Vector2 WallJumpForce => _wallJumpForce;
        public float WallJumpMoveLockDuration => _wallJumpMoveLockDuration;
    }
}
