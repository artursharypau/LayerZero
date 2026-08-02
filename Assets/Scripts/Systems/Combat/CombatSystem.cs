using System;
using System.Collections.Generic;
using Systems.Damage;
using UnityEngine;

namespace Systems.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        private const int TargetsBufferCapacity = 3;

        [Header("Target detection")]
        [SerializeField] private Transform _targetCheckPoint;
        [SerializeField] [Min(0f)] private float _targetCheckRadius = 1f;
        [SerializeField] private LayerMask _targetLayerMask;

        private IAttackAnimatorEvents _animTriggers;

        private ContactFilter2D _filter;
        private List<Collider2D> _targetsBuffer;
        private DamageDefinition _activeAttackDefinition;

        public event Action<DamageInfo> Damaged;

        private void Awake()
        {
            _animTriggers = GetComponentInChildren<IAttackAnimatorEvents>();

            _filter = new ContactFilter2D();
            _targetsBuffer = new List<Collider2D>(TargetsBufferCapacity);

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_targetCheckPoint.position, _targetCheckRadius);
        }

        public void SetActiveAttackDefinition(DamageDefinition damageDefinition)
        {
            _activeAttackDefinition = damageDefinition;
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
            if (_activeAttackDefinition == null)
            {
                Debug.unityLogger.LogError($"{nameof(CombatSystem)}.{nameof(OnAttackHit)}", $"No active attack is set on '{name}'");
                return;
            }

            int count = UpdateTargets();
            if (count <= 0)
            {
                return;
            }

            DamageInfo damageInfo = DamageInfo.FromDefinition(_activeAttackDefinition, transform);
            for (int i = 0; i < count; i++)
            {
                if (_targetsBuffer[i].TryGetComponent(out IDamageReceiver receiver))
                {
                    receiver.TakeDamage(damageInfo);
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
