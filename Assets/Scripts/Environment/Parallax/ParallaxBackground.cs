using UnityEngine;

namespace LayerZero.Environment.Parallax
{
    public sealed class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer[] _layers;

        private Camera _camera;
        private float _previousCameraX;

        private void Awake()
        {
            _camera = Camera.main;

            foreach (ParallaxLayer layer in _layers)
            {
                layer.Initialize();
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

            foreach (ParallaxLayer layer in _layers)
            {
                layer.Move(distance);
                layer.Recycle(distance, leftEdge, rightEdge);
            }
        }
    }
}
