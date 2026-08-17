using System.Collections.Generic;
using LayerZero.Combat.Attack;
using LayerZero.Combat.Attack.Executors;
using LayerZero.Combat.Damage;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Combat
{
    public sealed class CombatSystem : MonoBehaviour, IInterruptibleAttack
    {
        private readonly Dictionary<AttackKind, IAttackExecutor> _executors = new();

        private bool _isCounterattackWindowOpen;
        private AttackDefinition _attackDefinition;

        private IAttackEvents _attackEvents;
        private IAttackParryWindowEvents _attackParryWindowEvents;

        private void Awake()
        {
            foreach (IAttackExecutor executor in this.GetRequiredComponentsInChildren<IAttackExecutor>())
            {
                if (!_executors.TryAdd(executor.Kind, executor))
                {
                    GameLog.Warning(this, $"'{name}' has more than one executor for '{executor.Kind}'. Extra ones are ignored.");
                    continue;
                }

                executor.Initialize(transform);
            }

            _attackEvents = this.GetRequiredComponentInChildren<IAttackEvents>();
            _attackParryWindowEvents = GetComponentInChildren<IAttackParryWindowEvents>();
        }

        private void OnEnable()
        {
            _attackEvents.AttackHit += OnAttackAttackHit;
            _attackParryWindowEvents.AttackParryWindowOpened += OnParryWindowAttackParryWindowOpened;
            _attackParryWindowEvents.AttackParryWindowClosed += OnParryWindowAttackParryWindowClosed;
        }

        private void OnDisable()
        {
            _attackEvents.AttackHit -= OnAttackAttackHit;
            _attackParryWindowEvents.AttackParryWindowOpened -= OnParryWindowAttackParryWindowOpened;
            _attackParryWindowEvents.AttackParryWindowClosed -= OnParryWindowAttackParryWindowClosed;

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

            _attackDefinition = attack;
        }

        public void Disarm()
        {
            _attackDefinition = null;
        }

        public bool IsInRange(AttackKind kind, Transform target)
        {
            return target && _executors.TryGetValue(kind, out IAttackExecutor executor) && executor.IsInRange(target);
        }

        public bool TryInterrupt(DamageInfo damageInfo)
        {
            if (!_isCounterattackWindowOpen || _attackDefinition == null)
            {
                return false;
            }

            if (!IsInRange(_attackDefinition.Kind, damageInfo.Attacker))
            {
                return false;
            }

            Disarm();
            return true;
        }

        private void OnAttackAttackHit()
        {
            if (_attackDefinition == null)
            {
                return;
            }

            if (_executors.TryGetValue(_attackDefinition.Kind, out IAttackExecutor executor))
            {
                executor.Execute(_attackDefinition.Damage);
            }
        }

        private void OnParryWindowAttackParryWindowOpened()
        {
            _isCounterattackWindowOpen = true;
        }

        private void OnParryWindowAttackParryWindowClosed()
        {
            _isCounterattackWindowOpen = false;
        }
    }
}
