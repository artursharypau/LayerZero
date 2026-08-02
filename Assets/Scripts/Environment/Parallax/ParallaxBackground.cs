using UnityEngine;

namespace Environment.Parallax
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer[] _layers;

        private Camera _camera;
        private float _previousCameraPositionX;

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
            _previousCameraPositionX = _camera.transform.position.x;
        }

        private void LateUpdate()
        {
            float currentCameraPositionX = _camera.transform.position.x;
            float distance = currentCameraPositionX - _previousCameraPositionX;
            _previousCameraPositionX = currentCameraPositionX;

            float cameraHalfWidth = _camera.orthographicSize * _camera.aspect;
            float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
            float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

            foreach (ParallaxLayer layer in _layers)
            {
                layer.Move(distance);
                layer.LoopBackground(distance, cameraLeftEdge, cameraRightEdge);
            }
        }
    }
}
