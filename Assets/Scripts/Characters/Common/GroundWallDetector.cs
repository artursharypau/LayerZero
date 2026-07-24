using System;
using Infrastructure.Tick;
using Infrastructure.Utils;
using UnityEngine;

namespace Characters.Common
{
    [Serializable]
    public class GroundWallDetector : ITickable
    {
        [SerializeField] private Transform[] _groundCheckPoints;
        [SerializeField] private float _groundCheckDistance = 1.35f;
        [SerializeField] private Transform[] _wallCheckPoints;
        [SerializeField] private float _wallCheckDistance = 0.5f;

        private IFacing _facing;

        public bool IsGrounded { get; private set; }
        public bool IsWalled { get; private set; }

        public void Initialize(IFacing owner)
        {
            _facing = owner;
        }

        public void Tick(float deltaTime)
        {
            Vector2 direction = Mathf.Approximately(_facing.FacingDirection, 1f) ? Vector2.right : Vector2.left;

            IsGrounded = true;
            IsWalled = true;

            foreach (Transform point in _groundCheckPoints)
            {
                if (!Physics2D.Raycast(point.position, Vector2.down, _groundCheckDistance, LayerMaskProvider.Ground))
                {
                    IsGrounded = false;
                    break;
                }
            }

            foreach (Transform point in _wallCheckPoints)
            {
                if (!Physics2D.Raycast(point.position, direction, _wallCheckDistance, LayerMaskProvider.Ground))
                {
                    IsWalled = false;
                    break;
                }
            }
        }

        public void DrawGizmos()
        {
            if (_facing == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            foreach (Transform point in _groundCheckPoints)
            {
                Gizmos.DrawLine(point.position, point.position + new Vector3(0f, -_groundCheckDistance));
            }

            foreach (Transform point in _wallCheckPoints)
            {
                Gizmos.DrawLine(point.position, point.position + new Vector3(_wallCheckDistance * _facing.FacingDirection, 0f));
            }
        }
    }
}
