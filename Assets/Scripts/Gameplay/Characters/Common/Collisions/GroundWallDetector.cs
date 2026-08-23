using System;
using LayerZero.Core.Collisions;
using LayerZero.Gameplay.Characters.Common.Movement;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Collisions
{
    [Serializable]
    internal sealed class GroundWallDetector
    {
        [SerializeField] private RayProbe _groundProbe = new();
        [SerializeField] private RayProbe _wallProbe = new();
        [SerializeField] private LayerMask _solidMask;

        private IPositioned _positioned;

        public bool IsGrounded { get; private set; }
        public bool IsWalled { get; private set; }

        public void Initialize(IPositioned positioned)
        {
            _positioned = positioned;
        }

        public void Refresh()
        {
            if (_positioned == null)
            {
                return;
            }

            LayerMask mask = _solidMask.Or(GameLayers.Ground);

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
