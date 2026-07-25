using System;
using Characters.Common;
using Characters.Common.Extensions;
using Core.Tick;
using Core.Utils;
using Systems.Damage;
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

        private IFacing _facing;
        private IPositioned _positioned;
        private CountdownTimer _alertTimer;
        private CountdownTimer _checkTimer;

        public void Initialize(IFacing facing, IPositioned position)
        {
            _facing = facing;
            _positioned = position;

            _alertTimer = new CountdownTimer();
            _checkTimer = new CountdownTimer();

            _checkTimer.Start(_checkCooldown);
        }

        public event Action TargetFound;
        public event Action TargetLost;

        public Transform Current { get; private set; }
        public bool IsBehind => Current && !Mathf.Approximately(Direction, _facing.FacingDirection);

        public float Direction
        {
            get
            {
                if (!Current)
                {
                    return 0f;
                }

                return Current.position.x > _positioned.Position.x ? 1 : -1;
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
            if (_facing == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                _checkPoint.position,
                _checkPoint.position + new Vector3(_checkDistance * _facing.FacingDirection, 0f));
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
                _facing.GetVector(),
                _checkDistance,
                LayerMaskProvider.Player | LayerMaskProvider.Ground);

            return raycast.collider && LayerMaskProvider.Contains(raycast.collider.gameObject.layer, LayerMaskProvider.Player)
                ? raycast
                : default;
        }
    }
}
