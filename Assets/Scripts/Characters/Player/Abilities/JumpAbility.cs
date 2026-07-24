using System;
using UnityEngine;

namespace Characters.Player.Abilities
{
    [Serializable]
    public class JumpAbility
    {
        [SerializeField] private ushort _count = 2;
        [SerializeField] private float _force = 13f;
        [SerializeField] private Vector2 _wallJumpForce = new(6f, 12f);
        [SerializeField] private float _wallJumpMoveLockDuration = 0.2f;

        private ushort _available;

        public JumpAbility()
        {
            _available = _count;
        }

        public float Force => _force;
        public Vector2 WallJumpForce => _wallJumpForce;
        public float WallJumpMoveLockDuration => _wallJumpMoveLockDuration;
        public bool HasJumpsLeft => _available > 0;

        public void Consume()
        {
            if (_available > 0)
            {
                --_available;
            }
        }

        public void Reset()
        {
            _available = _count;
        }
    }
}
