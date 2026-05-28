using System;
using UnityEngine;

namespace Paralax
{
    [Serializable]
    public class ParallaxLayer
    {
        private const float DistanceThreshold = 0.1f;

        [SerializeField] private Transform _root;
        [SerializeField] private float _multiplier;

        private float _spriteWidth;
        private Transform[] _spritesTransforms;

        private int _leftIndex;
        private int _rightIndex;

        public void Initialize()
        {
            SpriteRenderer[] spriteRenderers = _root.GetComponentsInChildren<SpriteRenderer>();
            Array.Sort(spriteRenderers, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

            _spriteWidth = spriteRenderers[0].bounds.size.x;
            _spritesTransforms = new Transform[spriteRenderers.Length];

            for (int i = 0; i < _spritesTransforms.Length; i++)
            {
                _spritesTransforms[i] = spriteRenderers[i].transform;
            }

            _leftIndex = 0;
            _rightIndex = _spritesTransforms.Length - 1;
        }

        public void Move(float distance)
        {
            if (Mathf.Abs(distance) > Mathf.Epsilon)
            {
                _root.position += Vector3.right * (distance * _multiplier);
            }
        }

        public void LoopBackground(float distance, float cameraLeftEdge, float cameraRightEdge)
        {
            float spriteHalfWidth = _spriteWidth / 2;
            float leftSpriteRightEdge = _spritesTransforms[_leftIndex].position.x + spriteHalfWidth;
            float rightSpriteLeftEdge = _spritesTransforms[_rightIndex].position.x - spriteHalfWidth;

            if (distance > DistanceThreshold && cameraLeftEdge > leftSpriteRightEdge)
            {
                _spritesTransforms[_leftIndex].position = _spritesTransforms[_rightIndex].position + Vector3.right * _spriteWidth;

                _rightIndex = _leftIndex;
                _leftIndex = (_leftIndex + 1) % _spritesTransforms.Length;
            }
            else if (distance < -DistanceThreshold && cameraRightEdge < rightSpriteLeftEdge)
            {
                _spritesTransforms[_rightIndex].position = _spritesTransforms[_leftIndex].position + Vector3.left * _spriteWidth;

                _leftIndex = _rightIndex;
                _rightIndex = (_rightIndex - 1 + _spritesTransforms.Length) % _spritesTransforms.Length;
            }
        }
    }
}
