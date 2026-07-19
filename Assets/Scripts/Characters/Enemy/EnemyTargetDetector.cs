using System;
using Characters.Common;
using Core.Tick;
using Core.Utils;
using Systems.Combat;
using UnityEngine;

namespace Characters.Enemy
{
    [Serializable]
    public class EnemyTargetDetector : ITickable
    {
        [SerializeField] private float _alertDuration = 5f;
        [SerializeField] private float _checkCooldown = 0.5f;

        [Header("Front detection")]
        [SerializeField] private Transform _checkPoint;
        [SerializeField] private float _checkDistance = 13f;

        private IMovable _owner;
        private CountdownTimer _alertTimer;
        private CountdownTimer _checkTimer;

        public void Initialize(IMovable owner)
        {
            _owner = owner;

            _alertTimer = new CountdownTimer();
            _checkTimer = new CountdownTimer();

            _checkTimer.Start(_checkCooldown);
        }

        public event Action TargetFound;
        public event Action TargetLost;

        public Transform Current { get; private set; }
        public bool IsBehind => Current && !Mathf.Approximately(Direction, _owner.FacingDirection);

        public float Direction
        {
            get
            {
                if (!Current)
                {
                    return 0f;
                }

                return Current.position.x > _owner.RB.position.x ? 1 : -1;
            }
        }

        public void Tick(float deltaTime)
        {
            _alertTimer.Tick(Time.deltaTime);
            _checkTimer.Tick(Time.deltaTime);

            if (_checkTimer.IsExpired)
            {
                UpdateCurrent();
                _checkTimer.Start(_checkCooldown);
            }
        }

        public void DamageAlert(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player)
            {
                SetTarget(damageInfo.AttackerTransform);
            }
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                _checkPoint.position,
                _checkPoint.position + new Vector3(_checkDistance * _owner.FacingDirection, 0f));
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
                SetTarget(current);
            }

            if (_alertTimer.IsExpired)
            {
                if (Current)
                {
                    TargetLost?.Invoke();
                }

                Current = null;
            }
        }

        private void SetTarget(Transform target)
        {
            if (!Current)
            {
                TargetFound?.Invoke();
            }

            Current = target;
            _alertTimer.Start(_alertDuration);
        }

        private RaycastHit2D CheckForTargetInFront()
        {
            RaycastHit2D raycast = Physics2D.Raycast(
                _checkPoint.position,
                Vector2.right * _owner.FacingDirection,
                _checkDistance,
                LayerMaskProvider.Player | LayerMaskProvider.Ground);

            return raycast.collider && LayerMaskProvider.Contains(raycast.collider.gameObject.layer, LayerMaskProvider.Player)
                ? raycast
                : default;
        }
    }
}
