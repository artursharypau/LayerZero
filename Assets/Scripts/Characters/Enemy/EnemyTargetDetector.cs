using System;
using Characters.Common;
using Core.Utils;
using Systems.Combat;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyTargetDetector : MonoBehaviour
    {
        [SerializeField] private float _alertDuration = 5f;
        [SerializeField] private float _checkCooldown = 0.5f;

        [Header("Front detection")]
        [SerializeField] private Transform _checkPoint;
        [SerializeField] private float _checkDistance = 13f;

        private IMovable _controller;
        private CountdownTimer _alertTimer;
        private CountdownTimer _checkTimer;

        public event Action TargetFound;
        public event Action TargetLost;

        public Transform Current { get; private set; }
        public bool IsBehind => Current && !Mathf.Approximately(Direction, _controller.FacingDirection);

        public float Direction
        {
            get
            {
                if (!Current)
                {
                    return 0f;
                }

                return Current.position.x > transform.position.x ? 1 : -1;
            }
        }

        private void Awake()
        {
            _controller = GetComponent<IMovable>();
            _alertTimer = new CountdownTimer();
            _checkTimer = new CountdownTimer();
        }

        private void Start()
        {
            _checkTimer.Start(_checkCooldown);
        }

        private void Update()
        {
            _alertTimer.Tick(Time.deltaTime);

            if (_checkTimer.Tick(Time.deltaTime))
            {
                UpdateCurrent();
                _checkTimer.Start(_checkCooldown);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                _checkPoint.position,
                _checkPoint.position + new Vector3(_checkDistance * _controller.FacingDirection, 0f));
        }

        public void DamageAlert(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player)
            {
                if (!Current)
                {
                    TargetFound?.Invoke();
                }

                Current = damageInfo.AttackerTransform;
                _alertTimer.Start(_alertDuration);
            }
        }

        private void UpdateCurrent()
        {
            Transform current = null;

            RaycastHit2D targetInFront = CheckForTargetInFront();
            if (targetInFront.collider)
            {
                current = targetInFront.transform;
            }

            if (current)
            {
                if (!Current)
                {
                    TargetFound?.Invoke();
                }

                Current = current;
                _alertTimer.Start(_alertDuration);
            }

            if (!_alertTimer.IsRunning)
            {
                if (Current)
                {
                    TargetLost?.Invoke();
                }

                Current = null;
            }
        }

        private RaycastHit2D CheckForTargetInFront()
        {
            RaycastHit2D raycast = Physics2D.Raycast(
                _checkPoint.position,
                Vector2.right * _controller.FacingDirection,
                _checkDistance,
                LayerMaskProvider.Player | LayerMaskProvider.Ground);

            return raycast.collider && LayerMaskProvider.Contains(raycast.collider.gameObject.layer, LayerMaskProvider.Player)
                ? raycast
                : default;
        }
    }
}
