using System.Collections.Generic;
using Core.Animation;
using UnityEngine;

namespace Systems.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Target detection")]
        [SerializeField] private Transform _targetCheckPoint;
        [SerializeField] private float _targetCheckRadius = 1f;
        [SerializeField] private LayerMask _targetLayerMask;

        [Header("Damage details")]
        [SerializeField] private int _damageAmount = 10;
        [SerializeField] private DamageSource _damageSource;

        private AnimatorTriggers _animTriggers;

        private ContactFilter2D _filter;
        private List<Collider2D> _targetsBuffer;

        private void Awake()
        {
            _animTriggers = GetComponentInChildren<AnimatorTriggers>();

            _filter = new ContactFilter2D();
            _targetsBuffer = new List<Collider2D>(3);

            _filter.SetLayerMask(_targetLayerMask);
        }

        private void OnEnable()
        {
            _animTriggers.AttackHit += OnAttackHit;
        }

        private void OnDisable()
        {
            _animTriggers.AttackHit -= OnAttackHit;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_targetCheckPoint.position, _targetCheckRadius);
        }

        public bool HasTargets()
        {
            return UpdateTargets() > 0;
        }

        private void OnAttackHit()
        {
            int count = UpdateTargets();

            for (int i = 0; i < count; i++)
            {
                if (_targetsBuffer[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(new DamageInfo(_damageAmount, _damageSource, transform));
                }
            }
        }

        private int UpdateTargets()
        {
            _targetsBuffer.Clear();
            return Physics2D.OverlapCircle(_targetCheckPoint.position, _targetCheckRadius, _filter, _targetsBuffer);
        }
    }
}
