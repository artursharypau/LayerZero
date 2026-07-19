using System;
using Core.Tick;
using Core.Utils;
using UnityEngine;

namespace Characters.Common
{
    [Serializable]
    public class GroundWallDetector : ITickable
    {
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckDistance = 1.35f;
        [SerializeField] private Transform[] _wallCheckPoints;
        [SerializeField] private float _wallCheckDistance = 0.5f;

        private IMovable _owner;

        public bool IsGrounded { get; private set; }
        public bool IsWalled { get; private set; }

        public void Initialize(IMovable owner)
        {
            _owner = owner;
        }

        public void Tick(float deltaTime)
        {
            Vector2 direction = Mathf.Approximately(_owner.FacingDirection, 1f) ? Vector2.right : Vector2.left;

            IsGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance, LayerMaskProvider.Ground);
            IsWalled = false;

            foreach (Transform point in _wallCheckPoints)
            {
                if (Physics2D.Raycast(point.position, direction, _wallCheckDistance, LayerMaskProvider.Ground))
                {
                    IsWalled = true;
                    break;
                }
            }
        }

        public void DrawGizmos()
        {
            Gizmos.DrawLine(_groundCheckPoint.position, _groundCheckPoint.position + new Vector3(0f, -_groundCheckDistance));
            foreach (Transform wallCheckPoint in _wallCheckPoints)
            {
                Gizmos.DrawLine(
                    wallCheckPoint.position,
                    wallCheckPoint.position + new Vector3(_wallCheckDistance * _owner.FacingDirection, 0f));
            }
        }
    }
}
