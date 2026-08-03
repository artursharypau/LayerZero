using LayerZero.Combat.Damage;
using LayerZero.Core.Collisions;
using LayerZero.Core.Pooling;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    public sealed class ProjectileAttackExecutor : MonoBehaviour, IAttackExecutor
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _muzzle;
        [SerializeField] [Min(0f)] private float _projectileSpeed = 12f;

        [Header("Range")]
        [SerializeField] [Min(0f)] private float _minRange = 3f;
        [SerializeField] [Min(0f)] private float _maxRange = 12f;

        [Header("Layers")]
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _blockerMask;

        [Header("Pool")]
        [SerializeField] [Min(0)] private int _prewarm = 4;

        private ComponentPool<Projectile> _pool;
        private Transform _owner;

        public AttackKind Kind => AttackKind.Ranged;
        public float MinRange => _minRange;
        public float MaxRange => _maxRange;

        public void Initialize(Transform owner)
        {
            _owner = owner;
            if (!_muzzle)
            {
                _muzzle = owner;
            }

            if (_projectilePrefab)
            {
                _pool = new ComponentPool<Projectile>(_projectilePrefab, null, _prewarm);
            }
        }

        public bool IsInRange(Transform target)
        {
            if (!target || !_muzzle)
            {
                return false;
            }

            float distance = Mathf.Abs(target.position.x - _muzzle.position.x);
            if (distance < _minRange || distance > _maxRange)
            {
                return false;
            }

            Vector2 origin = _muzzle.position;
            Vector2 toTarget = (Vector2)target.position - origin;

            RaycastHit2D hit = Physics2D.Raycast(origin, toTarget.normalized, toTarget.magnitude, _targetMask | _blockerMask);
            return hit.collider && _targetMask.Contains(hit.collider.gameObject);
        }

        public void Execute(DamageDefinition damage)
        {
            if (_pool == null || !_muzzle)
            {
                return;
            }

            Vector2 direction = new(Mathf.Sign(_owner ? _owner.right.x : _muzzle.right.x), 0f);

            Projectile projectile = _pool.Get(_muzzle.position, Quaternion.identity);
            projectile.Launch(direction, _projectileSpeed, damage, _owner ? _owner : transform, _targetMask, _blockerMask, Release);
        }

        private void Release(Projectile projectile)
        {
            _pool?.Release(projectile);
        }

        private void OnDestroy()
        {
            _pool?.Dispose();
            _pool = null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_muzzle)
            {
                return;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_muzzle.position, _muzzle.position + _muzzle.right * _maxRange);
        }
    }
}
