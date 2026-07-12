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

        private AnimatorTriggers _animTriggers;

        private void Awake()
        {
            _animTriggers = GetComponentInChildren<AnimatorTriggers>();
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

        private void OnAttackHit()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(_targetCheckPoint.position, _targetCheckRadius, _targetLayerMask);
        }
    }
}
