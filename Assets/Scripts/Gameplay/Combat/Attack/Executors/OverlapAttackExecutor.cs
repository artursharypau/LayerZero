using System.Collections.Generic;
using LayerZero.Core.EventBus;
using LayerZero.Core.Extensions;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Combat.Events;
using UnityEngine;
using VContainer;

namespace LayerZero.Gameplay.Combat.Attack.Executors
{
    internal sealed class OverlapAttackExecutor : MonoBehaviour, IAttackExecutor
    {
        private const int TargetsBufferCapacity = 8;

        [SerializeField] private AttackKind _kind = AttackKind.Melee;
        [SerializeField] private Transform _origin;
        [SerializeField] [Min(0f)] private float _radius = 1f;
        [SerializeField] private LayerMask _targetMask;

        private readonly List<Collider2D> _targets = new(TargetsBufferCapacity);

        private ContactFilter2D _filter;
        private IGameEventBus _eventBus;

        public AttackKind Kind => _kind;

        [Inject]
        public void Construct(IGameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(_targetMask);
        }

        public int FindTargets(List<Collider2D> results)
        {
            results.Clear();
            return _origin ? Physics2D.OverlapCircle(_origin.position, _radius, _filter, results) : 0;
        }

        public bool IsInRange(Transform target)
        {
            if (!target)
            {
                return false;
            }

            int count = FindTargets(_targets);
            for (int i = 0; i < count; i++)
            {
                if (_targets[i].transform == target || _targets[i].transform.IsChildOf(target))
                {
                    return true;
                }
            }

            return false;
        }

        public void Execute(DamageDefinition damage)
        {
            int count = FindTargets(_targets);
            if (count <= 0)
            {
                return;
            }

            DamageInfo damageInfo = DamageInfo.FromDefinition(damage, transform);
            for (int i = 0; i < count; i++)
            {
                if (!_targets[i].TryGetRequiredComponent(out IDamageReceiver receiver) || !receiver.TakeDamage(damageInfo))
                {
                    continue;
                }

                RaiseAttackHit(_targets[i]);
            }
        }

        private void RaiseAttackHit(Collider2D target)
        {
            _eventBus?.Raise(new AttackHitEvent(target.bounds.center));
        }

        private void OnDrawGizmosSelected()
        {
            if (!_origin)
            {
                return;
            }

            Gizmos.color = _kind == AttackKind.Counterattack ? Color.cyan : Color.red;
            Gizmos.DrawWireSphere(_origin.position, _radius);
        }
    }
}
