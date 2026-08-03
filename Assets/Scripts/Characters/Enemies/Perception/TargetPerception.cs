using System;
using LayerZero.Characters.Common;
using LayerZero.Characters.Common.Movement;
using LayerZero.Characters.Enemies.Config;
using LayerZero.Combat.Damage;
using LayerZero.Core.Collisions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Perception
{
    /// <summary>
    /// The enemy's senses, as a module: line-of-sight scanning on an interval, memory of the last
    /// target, and aggro from being hit. States only read the results.
    /// <para>
    /// A different archetype gets different senses by swapping this module or its settings -
    /// no changes to the shared controller.
    /// </para>
    /// </summary>
    public sealed class TargetPerception : CharacterModule
    {
        private readonly PerceptionSettings _settings;
        private readonly Transform _sightOrigin;
        private readonly CountdownTimer _alertTimer = new();
        private readonly CountdownTimer _scanTimer = new();

        private IPositioned _positioned;
        private LayerMask _targetMask;
        private LayerMask _blockerMask;

        public TargetPerception(PerceptionSettings settings, Transform sightOrigin)
        {
            _settings = settings;
            _sightOrigin = sightOrigin;
        }

        public event Action TargetAcquired;
        public event Action TargetLost;

        public Transform Target { get; private set; }
        public bool HasTarget => Target;

        /// <summary>-1 / +1 towards the target, 0 when there is none.</summary>
        public float DirectionToTarget
        {
            get
            {
                if (!Target || _positioned == null)
                {
                    return 0f;
                }

                return Target.position.x > _positioned.Position.x ? 1f : -1f;
            }
        }

        public float HorizontalDistanceToTarget =>
            Target && _positioned != null ? Mathf.Abs(Target.position.x - _positioned.Position.x) : float.PositiveInfinity;

        public bool IsTargetBehind =>
            Target && _positioned != null && !Mathf.Approximately(DirectionToTarget, _positioned.FacingDirection);

        protected override void OnInitialize()
        {
            _positioned = Owner.Movement;
            _targetMask = _settings.TargetMask.value != 0 ? _settings.TargetMask : GameLayers.Player;
            _blockerMask = _settings.BlockerMask.value != 0 ? _settings.BlockerMask : GameLayers.Ground;

            _scanTimer.Start(_settings.ScanInterval);
        }

        public override void FixedTick(float deltaTime)
        {
            _alertTimer.Tick(deltaTime);
            _scanTimer.Tick(deltaTime);

            if (!_scanTimer.IsExpired)
            {
                return;
            }

            _scanTimer.Start(_settings.ScanInterval);
            Scan();
        }

        public override void Disable()
        {
            ClearTarget();
        }

        /// <summary>Being hit from outside the field of view still pulls aggro.</summary>
        public void NotifyDamaged(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player && damageInfo.Attacker)
            {
                SetTarget(damageInfo.Attacker);
            }
        }

        public override void DrawGizmos()
        {
            if (!_sightOrigin || _positioned == null)
            {
                return;
            }

            Gizmos.color = HasTarget ? Color.red : Color.gray;
            Gizmos.DrawLine(
                _sightOrigin.position,
                _sightOrigin.position + new Vector3(_settings.SightDistance * _positioned.FacingDirection, 0f));
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
                ClearTarget();
            }
        }

        private Transform CastForTarget()
        {
            if (!_sightOrigin || _positioned == null)
            {
                return null;
            }

            RaycastHit2D hit = Physics2D.Raycast(
                _sightOrigin.position,
                _positioned.FacingVector,
                _settings.SightDistance,
                _targetMask | _blockerMask);

            return hit.collider && _targetMask.Contains(hit.collider.gameObject) ? hit.transform : null;
        }

        private void SetTarget(Transform target)
        {
            bool isNew = !Target;

            Target = target;
            _alertTimer.Start(_settings.AlertDuration);

            if (isNew)
            {
                TargetAcquired?.Invoke();
            }
        }

        private void ClearTarget()
        {
            if (!Target)
            {
                return;
            }

            Target = null;
            TargetLost?.Invoke();
        }
    }
}
