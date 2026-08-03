using System;
using Characters.Common.Extensions;
using Characters.Common.Movement;
using Core.Utils;
using UnityEngine;

namespace Characters.Common.Detection
{
    [Serializable]
    public class GroundWallDetector
    {
        [SerializeField] private Transform[] _groundCheckPoints;
        [SerializeField] private float _groundCheckDistance = 0.2f;

        [SerializeField] private Transform[] _wallCheckPoints;
        [SerializeField] private float _wallCheckDistance = 0.5f;

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

            Vector2 wallDirection = _positioned.GetFacingDirectionVector();

            IsGrounded = AllRaysHit(_groundCheckPoints, Vector2.down, _groundCheckDistance, LayerMaskProvider.Ground);
            IsWalled = AllRaysHit(_wallCheckPoints, wallDirection, _wallCheckDistance, LayerMaskProvider.Ground);
        }

        public void DrawGizmos()
        {
            if (_positioned == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            DrawRays(_groundCheckPoints, Vector2.down * _groundCheckDistance);
            DrawRays(_wallCheckPoints, Vector2.right * (_wallCheckDistance * _positioned.FacingDirection));
        }

        private static bool AllRaysHit(Transform[] points, Vector2 direction, float distance, LayerMask mask)
        {
            if (points == null || points.Length == 0)
            {
                return false;
            }

            foreach (Transform point in points)
            {
                if (!Physics2D.Raycast(point.position, direction, distance, mask))
                {
                    return false;
                }
            }

            return true;
        }

        private static void DrawRays(Transform[] points, Vector3 offset)
        {
            if (points == null)
            {
                return;
            }

            foreach (Transform point in points)
            {
                Gizmos.DrawLine(point.position, point.position + offset);
            }
        }
    }
}
