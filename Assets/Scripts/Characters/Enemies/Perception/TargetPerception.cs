using LayerZero.Characters.Common.Movement;
using LayerZero.Characters.Enemies.Config;
using LayerZero.Combat.Damage;
using LayerZero.Core.Collisions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Perception
{
    public sealed class TargetPerception
    {
        private readonly PerceptionConfig _config;
        private readonly Transform _sightOrigin;
        private readonly IPositioned _positioned;
        private readonly LayerMask _targetMask;
        private readonly LayerMask _blockerMask;

        private Countdown _alertTimer;
        private Countdown _scanTimer;

        public TargetPerception(IPositioned positioned, PerceptionConfig config, Transform sightOrigin)
        {
            _positioned = positioned;
            _config = config;
            _sightOrigin = sightOrigin;

            _targetMask = _config.TargetMask.Or(GameLayers.Player);
            _blockerMask = _config.BlockerMask.Or(GameLayers.Ground);

            _scanTimer.Start(_config.ScanInterval);
        }

        public Transform Target { get; private set; }
        public bool HasTarget => Target;

        public bool IsTargetBehind =>
            Target && !Mathf.Approximately(DirectionToTarget, _positioned.FacingDirection);

        public float DirectionToTarget
        {
            get
            {
                if (!Target)
                {
                    return 0f;
                }

                return Target.position.x > _positioned.Position.x ? 1f : -1f;
            }
        }

        public void FixedUpdate()
        {
            if (!_scanTimer.IsExpired)
            {
                return;
            }

            _scanTimer.Start(_config.ScanInterval);
            Scan();
        }

        public void ForgetTarget()
        {
            Target = null;
        }

        public void NotifyDamaged(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player && damageInfo.Attacker)
            {
                SetTarget(damageInfo.Attacker);
            }
        }

        public void DrawGizmos()
        {
            if (!_sightOrigin)
            {
                return;
            }

            Gizmos.color = HasTarget ? Color.red : Color.gray;
            Gizmos.DrawLine(
                _sightOrigin.position,
                _sightOrigin.position + new Vector3(_config.SightDistance * _positioned.FacingDirection, 0f));
        }

        private void Scan()
        {
            Transform seen = CastForTarget();
            if (seen)
            {
                SetTarget(seen);
                return;
            }

            if (_alertTimer.IsExpired)
            {
                ForgetTarget();
            }
        }

        private Transform CastForTarget()
        {
            if (!_sightOrigin)
            {
                return null;
            }

            RaycastHit2D hit = Physics2D.Raycast(
                _sightOrigin.position,
                _positioned.FacingVector,
                _config.SightDistance,
                _targetMask | _blockerMask);

            return hit.collider && _targetMask.Contains(hit.collider.gameObject) ? hit.transform : null;
        }

        private void SetTarget(Transform target)
        {
            Target = target;
            _alertTimer.Start(_config.AlertDuration);
        }
    }
}
