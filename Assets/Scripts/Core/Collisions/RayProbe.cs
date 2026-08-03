using System;
using UnityEngine;

namespace LayerZero.Core.Collisions
{
    /// <summary>
    /// A set of raycast origins sampled in the same direction - the reusable building block
    /// behind ground / wall / ledge checks. Serialize one per check on the character prefab.
    /// </summary>
    [Serializable]
    public sealed class RayProbe
    {
        [SerializeField] private Transform[] _origins = Array.Empty<Transform>();
        [SerializeField] [Min(0f)] private float _distance = 0.2f;

        public float Distance => _distance;
        public bool IsConfigured => _origins is { Length: > 0 };

        /// <summary>True only when every origin hits something on <paramref name="mask" />.</summary>
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

        /// <summary>True when at least one origin hits something on <paramref name="mask" />.</summary>
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
