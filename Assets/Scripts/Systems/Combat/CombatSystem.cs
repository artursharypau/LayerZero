using System.Collections.Generic;
using Systems.Damage;
using UnityEngine;

namespace Systems.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Target detection")]
        [SerializeField] private Transform _targetCheckPoint;
        [SerializeField] private float _targetCheckRadius = 1f;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private DamageDefinition _damageDefinition;

        private IAttackAnimatorEvents _animTriggers;

        private ContactFilter2D _filter;
        private List<Collider2D> _targetsBuffer;

        private void Awake()
        {
            _animTriggers = GetComponentInChildren<IAttackAnimatorEvents>();

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

        public bool IsInRange(Transform target)
        {
            if (!target)
            {
                return false;
            }

            int count = UpdateTargets();
            for (int i = 0; i < count; i++)
            {
                if (_targetsBuffer[i].transform == target)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnAttackHit()
        {
            int count = UpdateTargets();
            if (count == 0)
            {
                return;
            }

            DamageInfo damageInfo = DamageInfo.FromDefinition(_damageDefinition, transform);
            for (int i = 0; i < count; i++)
            {
                if (_targetsBuffer[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageInfo);
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
