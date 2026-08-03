using System;
using LayerZero.Combat.Damage;
using LayerZero.Core.Collisions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    /// <summary>
    /// Pooled projectile. Carries the authoring damage and resolves it at impact using its own
    /// orientation, so knockback follows the flight direction rather than the shooter's facing.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float _lifetime = 3f;

        private readonly CountdownTimer _lifetimeTimer = new();

        private DamageDefinition _damage;
        private Transform _attacker;
        private LayerMask _targetMask;
        private LayerMask _blockerMask;
        private float _speed;
        private Action<Projectile> _despawn;
        private bool _isActive;

        public void Launch(
            Vector2 direction,
            float speed,
            DamageDefinition damage,
            Transform attacker,
            LayerMask targetMask,
            LayerMask blockerMask,
            Action<Projectile> despawn)
        {
            _damage = damage;
            _attacker = attacker;
            _targetMask = targetMask;
            _blockerMask = blockerMask;
            _speed = speed;
            _despawn = despawn;
            _isActive = true;

            transform.right = direction.normalized;
            _lifetimeTimer.Start(_lifetime);
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            transform.position += transform.right * (_speed * Time.deltaTime);

            _lifetimeTimer.Tick(Time.deltaTime);
            if (_lifetimeTimer.IsExpired)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive)
            {
                return;
            }

            if (_targetMask.Contains(other.gameObject))
            {
                if (other.TryGetComponent(out IDamageReceiver receiver))
                {
                    receiver.TakeDamage(DamageInfo.FromDefinition(_damage, _attacker, transform));
                }

                Despawn();
                return;
            }

            if (_blockerMask.Contains(other.gameObject))
            {
                Despawn();
            }
        }

        private void Despawn()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;

            Action<Projectile> despawn = _despawn;
            _despawn = null;

            if (despawn != null)
            {
                despawn(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
