using System.Collections.Generic;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    public sealed class CombatSystem : MonoBehaviour
    {
        private readonly Dictionary<AttackKind, IAttackExecutor> _executors = new();

        private IAttackEvents _attackEvents;
        private AttackDefinition _armedAttack;

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

            _attackEvents = this.GetRequiredInChildren<IAttackEvents>();
        }

        private void OnEnable()
        {
            _attackEvents.AttackHit += OnAttackHit;
        }

        private void OnDisable()
        {
            _attackEvents.AttackHit -= OnAttackHit;

            Disarm();
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

        public void Disarm()
        {
            _armedAttack = null;
        }

        public bool IsInRange(AttackKind kind, Transform target)
        {
            return target && _executors.TryGetValue(kind, out IAttackExecutor executor) && executor.IsInRange(target);
        }

        private void OnAttackHit()
        {
            if (_armedAttack == null)
            {
                return;
            }

            if (_executors.TryGetValue(_armedAttack.Kind, out IAttackExecutor executor))
            {
                executor.Execute(_armedAttack.Damage);
            }
        }
    }
}
