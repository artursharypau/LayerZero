using System;
using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Environment.Parallax
{
    [Serializable]
    public sealed class ParallaxLayer
    {
        private const float DistanceThreshold = 0.1f;

        [SerializeField] private Transform _root;

        [Tooltip("0 = pinned to the camera, 1 = moves with the world.")] [SerializeField] [Range(0f, 1f)]
        private float _multiplier = 0.5f;

        private Transform[] _sprites;
        private float _spriteWidth;
        private int _leftIndex;
        private int _rightIndex;

        public void Initialize()
        {
            if (!_root)
            {
                return;
            }

            SpriteRenderer[] renderers = _root.GetRequiredComponentsInChildren<SpriteRenderer>();
            if (renderers.Length == 0)
            {
                return;
            }

            Array.Sort(renderers, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

            _spriteWidth = renderers[0].bounds.size.x;
            _sprites = new Transform[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                _sprites[i] = renderers[i].transform;
            }

            _leftIndex = 0;
            _rightIndex = _sprites.Length - 1;
        }

        public void Move(float distance)
        {
            if (_root && Mathf.Abs(distance) > Mathf.Epsilon)
            {
                _root.position += Vector3.right * (distance * _multiplier);
            }
        }

        public void Recycle(float distance, float cameraLeftEdge, float cameraRightEdge)
        {
            if (_sprites?.Length > 0)
            {
                return;
            }

            float halfWidth = _spriteWidth * 0.5f;
            float leftSpriteRightEdge = _sprites[_leftIndex].position.x + halfWidth;
            float rightSpriteLeftEdge = _sprites[_rightIndex].position.x - halfWidth;

            if (distance > DistanceThreshold && cameraLeftEdge > leftSpriteRightEdge)
            {
                _sprites[_leftIndex].position = _sprites[_rightIndex].position + Vector3.right * _spriteWidth;

                _rightIndex = _leftIndex;
                _leftIndex = (_leftIndex + 1) % _sprites.Length;
            }
            else if (distance < -DistanceThreshold && cameraRightEdge < rightSpriteLeftEdge)
            {
                _sprites[_rightIndex].position = _sprites[_leftIndex].position + Vector3.left * _spriteWidth;

                _leftIndex = _rightIndex;
                _rightIndex = (_rightIndex - 1 + _sprites.Length) % _sprites.Length;
            }
        }
    }
}
