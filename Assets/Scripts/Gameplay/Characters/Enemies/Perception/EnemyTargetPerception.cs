using LayerZero.Core.Collisions;
using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Characters.Enemies.Config;
using LayerZero.Gameplay.Collisions;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies.Perception
{
    internal sealed class EnemyTargetPerception
    {
        private readonly PerceptionConfig _config;
        private readonly Transform _origin;
        private readonly IPositioned _positioned;

        private readonly LayerMask _targetMask;
        private readonly LayerMask _blockerMask;

        private Countdown _alertTimer;
        private Countdown _scanTimer;

        public EnemyTargetPerception(PerceptionConfig config, Transform origin, IPositioned positioned)
        {
            _config = config;
            _origin = origin;
            _positioned = positioned;

            _targetMask = _config.TargetMask.Or(GameLayers.Player);
            _blockerMask = _config.BlockerMask.Or(GameLayers.Ground);

            _scanTimer.Start(_config.ScanInterval);
        }

        public EnemyPerceivedTarget Target { get; private set; }
        public bool HasTarget => Target.IsValid;

        public bool IsTargetBehind =>
            HasTarget && !Mathf.Approximately(DirectionToTarget, _positioned.FacingDirection);

        public float DirectionToTarget
        {
            get
            {
                if (!HasTarget)
                {
                    return 0f;
                }

                return Target.Transform.position.x > _positioned.Position.x ? 1f : -1f;
            }
        }

        public void FixedUpdate()
        {
            if (!Target.IsValid)
            {
                ForgetTarget();
            }

            if (_scanTimer.IsExpired)
            {
                _scanTimer.Start(_config.ScanInterval);
                ScanForTarget();
            }
        }

        public void ForgetTarget()
        {
            Target = EnemyPerceivedTarget.None;
        }

        public void NotifyDamaged(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player && damageInfo.AttackerTransform)
            {
                SetTarget(damageInfo.AttackerTransform);
            }
        }

        public void DrawGizmos()
        {
            if (!_origin)
            {
                return;
            }

            Gizmos.color = HasTarget ? Color.red : Color.gray;
            Gizmos.DrawLine(
                _origin.position,
                _origin.position + new Vector3(_config.Distance * _positioned.FacingDirection, 0f));
        }

        private void ScanForTarget()
        {
            Transform targetTransform = CastForTarget();
            if (targetTransform)
            {
                SetTarget(targetTransform);
                return;
            }

            if (_alertTimer.IsExpired)
            {
                ForgetTarget();
            }
        }

        private Transform CastForTarget()
        {
            if (!_origin)
            {
                return null;
            }

            RaycastHit2D hit = Physics2D.Raycast(
                _origin.position,
                _positioned.FacingVector,
                _config.Distance,
                _targetMask | _blockerMask);

            return hit.collider && _targetMask.Contains(hit.collider.gameObject) ? hit.transform : null;
        }

        private void SetTarget(Transform transform)
        {
            IDamageReceiver damageReceiver = transform.GetRequiredComponentInParent<IDamageReceiver>();
            if (damageReceiver == null || damageReceiver.IsDead)
            {
                return;
            }

            Target = new EnemyPerceivedTarget(transform, damageReceiver);
            _alertTimer.Start(_config.AlertDuration);
        }
    }
}
