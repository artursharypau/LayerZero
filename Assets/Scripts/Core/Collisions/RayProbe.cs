using System;
using UnityEngine;

namespace LayerZero.Core.Collisions
{
    [Serializable]
    public sealed class RayProbe
    {
        [SerializeField] private Transform[] _origins = Array.Empty<Transform>();
        [SerializeField] [Min(0f)] private float _distance = 0.2f;

        private bool IsConfigured => _origins != null && _origins.Length > 0;

        public bool AllHit(Vector2 direction, LayerMask mask)
        {
            if (!IsConfigured)
            {
                return false;
            }

            foreach (Transform origin in _origins)
            {
                if (!origin || !Physics2D.Raycast(origin.position, direction, _distance, mask))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AnyHit(Vector2 direction, LayerMask mask)
        {
            if (!IsConfigured)
            {
                return false;
            }

            foreach (Transform origin in _origins)
            {
                if (origin && Physics2D.Raycast(origin.position, direction, _distance, mask))
                {
                    return true;
                }
            }

            return false;
        }

        public void DrawGizmos(Vector2 direction)
        {
            if (!IsConfigured)
            {
                return;
            }

            Vector3 offset = (Vector3)direction.normalized * _distance;
            foreach (Transform origin in _origins)
            {
                if (origin)
                {
                    Gizmos.DrawLine(origin.position, origin.position + offset);
                }
            }
        }
    }
}
