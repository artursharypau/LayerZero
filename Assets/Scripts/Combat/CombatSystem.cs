using System.Collections.Generic;
using LayerZero.Combat.Attack;
using LayerZero.Combat.Attack.Executors;
using LayerZero.Core.Diagnostics;
using UnityEngine;
using VContainer;

namespace LayerZero.Combat
{
    public sealed class CombatSystem : MonoBehaviour, IInterruptibleAttack
    {
        private readonly Dictionary<AttackKind, IAttackExecutor> _executors = new();

        private bool _isParryWindowOpen;
        private AttackDefinition _attackDefinition;

        private IAttackEvents _attackEvents;
        private IAttackParryWindowEvents _attackParryWindowEvents;

        [Inject]
        public void Construct(
            IReadOnlyList<IAttackExecutor> executors,
            IAttackEvents attackEvents,
            IAttackParryWindowEvents attackParryWindowEvents)
        {
            _attackEvents = attackEvents;
            _attackParryWindowEvents = attackParryWindowEvents;

            foreach (IAttackExecutor executor in executors)
            {
                if (!_executors.TryAdd(executor.Kind, executor))
                {
                    GameLog.Warning(this, $"'{name}' has more than one executor for '{executor.Kind}'. Extra ones are ignored.");
                }
            }
        }

        private void OnEnable()
        {
            _attackEvents.AttackHit += OnAttackHit;
            _attackParryWindowEvents.AttackParryWindowOpened += OnParryWindowOpened;
            _attackParryWindowEvents.AttackParryWindowClosed += OnParryWindowClosed;
        }

        private void OnDisable()
        {
            _attackEvents.AttackHit -= OnAttackHit;
            _attackParryWindowEvents.AttackParryWindowOpened -= OnParryWindowOpened;
            _attackParryWindowEvents.AttackParryWindowClosed -= OnParryWindowClosed;

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

            _isParryWindowOpen = false;
            _attackDefinition = attack;
        }

        public void Disarm()
        {
            _isParryWindowOpen = false;
            _attackDefinition = null;
        }

        public bool IsInRange(AttackKind kind, Transform target)
        {
            return target && _executors.TryGetValue(kind, out IAttackExecutor executor) && executor.IsInRange(target);
        }

        public bool TryParry()
        {
            if (!_executors.TryGetValue(AttackKind.Counterattack, out IAttackExecutor executor))
            {
                GameLog.Error(this, $"'{name}' has no executor for '{AttackKind.Counterattack}'.");
                return false;
            }

            bool isParried = false;

            List<Collider2D> targets = new(2);
            int count = executor.FindTargets(targets);

            for (int i = 0; i < count; i++)
            {
                if (targets[i].TryGetComponent(out IInterruptibleAttack attacker) && attacker.TryInterrupt(transform))
                {
                    isParried = true;
                }
            }

            return isParried;
        }

        bool IInterruptibleAttack.TryInterrupt(Transform parrier)
        {
            if (!_isParryWindowOpen || _attackDefinition == null || !IsInRange(_attackDefinition.Kind, parrier))
            {
                return false;
            }

            Disarm();
            return true;
        }

        private void OnAttackHit()
        {
            _isParryWindowOpen = false;

            if (_attackDefinition == null)
            {
                return;
            }

            if (_executors.TryGetValue(_attackDefinition.Kind, out IAttackExecutor executor))
            {
                executor.Execute(_attackDefinition.Damage);
            }
        }

        private void OnParryWindowOpened()
        {
            _isParryWindowOpen = _attackDefinition != null;
        }

        private void OnParryWindowClosed()
        {
            _isParryWindowOpen = false;
        }
    }
}
