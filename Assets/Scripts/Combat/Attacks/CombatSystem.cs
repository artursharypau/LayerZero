using System.Collections.Generic;
using LayerZero.Core.Diagnostics;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    public sealed class CombatSystem : MonoBehaviour
    {
        private readonly Dictionary<AttackKind, IAttackExecutor> _executors = new();

        private IAttackAnimatorEvents _animatorEvents;
        private AttackDefinition _armedAttack;

        public IReadOnlyDictionary<AttackKind, IAttackExecutor> Executors => _executors;

        private void Awake()
        {
            foreach (IAttackExecutor executor in GetComponentsInChildren<IAttackExecutor>(true))
            {
                if (!_executors.TryAdd(executor.Kind, executor))
                {
                    GameLog.Warning(this, $"'{name}' has more than one executor for '{executor.Kind}'. Extra ones are ignored.");
                    continue;
                }

                executor.Initialize(transform);
            }

            _animatorEvents = GetComponentInChildren<IAttackAnimatorEvents>(true);
            if (_animatorEvents == null)
            {
                GameLog.Error(this, $"'{name}' has no {nameof(IAttackAnimatorEvents)} in its hierarchy - attacks will never land.");
            }
        }

        private void OnEnable()
        {
            if (_animatorEvents != null)
            {
                _animatorEvents.AttackHit += OnAttackHit;
            }
        }

        private void OnDisable()
        {
            if (_animatorEvents != null)
            {
                _animatorEvents.AttackHit -= OnAttackHit;
            }

            _armedAttack = null;
        }

        public void Arm(AttackDefinition attack)
        {
            if (attack == null)
            {
                GameLog.Error(this, $"Tried to arm a null attack on '{name}'.");
                return;
            }

            if (!_executors.ContainsKey(attack.Kind))
            {
                GameLog.Error(this, $"'{name}' has no executor for '{attack.Kind}'.");
                return;
            }

            _armedAttack = attack;
        }

        public bool IsInRange(AttackKind kind, Transform target)
        {
            return target && _executors.TryGetValue(kind, out IAttackExecutor executor) && executor.IsInRange(target);
        }

        private void OnAttackHit()
        {
            if (_armedAttack == null)
            {
                GameLog.Error(this, $"Attack hit event fired on '{name}' with no armed attack.");
                return;
            }

            if (_executors.TryGetValue(_armedAttack.Kind, out IAttackExecutor executor))
            {
                executor.Execute(_armedAttack.Damage);
            }
        }
    }
}
