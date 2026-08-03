using System;
using LayerZero.Core.Collisions;
using UnityEngine;

namespace LayerZero.Characters.Common.Movement
{
    [Serializable]
    public sealed class GroundWallDetector
    {
        [SerializeField] private RayProbe _groundProbe = new();
        [SerializeField] private RayProbe _wallProbe = new();
        [SerializeField] private LayerMask _solidMask;

        private IPositioned _positioned;
        private bool _useDefaultMask;

        public bool IsGrounded { get; private set; }
        public bool IsWalled { get; private set; }

        public void Initialize(IPositioned positioned)
        {
            _positioned = positioned;
            _useDefaultMask = _solidMask.value == 0;
        }

        public void Refresh()
        {
            if (_positioned == null)
            {
                return;
            }

            LayerMask mask = _useDefaultMask ? GameLayers.Ground : _solidMask;

            IsGrounded = _groundProbe.AllHit(Vector2.down, mask);
            IsWalled = _wallProbe.AllHit(_positioned.FacingVector, mask);
        }

        public void DrawGizmos()
        {
            if (_positioned == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            _groundProbe.DrawGizmos(Vector2.down);
            _wallProbe.DrawGizmos(_positioned.FacingVector);
        }
    }
}
