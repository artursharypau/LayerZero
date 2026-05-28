using UnityEngine;

namespace Paralax
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer[] _layers;

        private Camera _camera;
        private float _previousCameraPositionX;
        private float _cameraHalfWidth;

        private void Awake()
        {
            _camera = Camera.main;
            _cameraHalfWidth = _camera!.orthographicSize * _camera.aspect;

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

            float cameraLeftEdge = currentCameraPositionX - _cameraHalfWidth;
            float cameraRightEdge = currentCameraPositionX + _cameraHalfWidth;

            foreach (ParallaxLayer layer in _layers)
            {
                layer.Move(distance);
                layer.LoopBackground(distance, cameraLeftEdge, cameraRightEdge);
            }
        }
    }
}
