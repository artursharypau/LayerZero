using UnityEngine;

namespace LayerZero.Presentation.Parallax
{
    internal sealed class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer[] _layers;

        private Camera _camera;
        private float _previousCameraX;

        private void Awake()
        {
            _camera = Camera.main;

            for (int i = 0; i < _layers.Length; i++)
            {
                _layers[i].Initialize();
            }
        }

        private void Start()
        {
            if (_camera)
            {
                _previousCameraX = _camera.transform.position.x;
            }
        }

        private void LateUpdate()
        {
            if (!_camera)
            {
                return;
            }

            float cameraX = _camera.transform.position.x;
            float distance = cameraX - _previousCameraX;
            _previousCameraX = cameraX;

            float halfWidth = _camera.orthographicSize * _camera.aspect;
            float leftEdge = cameraX - halfWidth;
            float rightEdge = cameraX + halfWidth;

            for (int i = 0; i < _layers.Length; i++)
            {
                ParallaxLayer layer = _layers[i];

                layer.Move(distance);
                layer.Recycle(distance, leftEdge, rightEdge);
            }
        }
    }
}
