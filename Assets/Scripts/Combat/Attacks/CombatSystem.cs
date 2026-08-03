using System.Collections.Generic;
using LayerZero.Core.Diagnostics;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    /// <summary>
    /// The character's combat facade. Holds one executor per <see cref="AttackKind" />, arms the
    /// attack a state is about to perform and fires it on the animation's hit event.
    /// <para>
    /// States only say "I am attacking with this definition"; nothing in a state knows whether
    /// that means a hitbox overlap or a projectile.
    /// </para>
    /// </summary>
    public sealed class CombatSystem : MonoBehaviour
    {
        private readonly Dictionary<AttackKind, IAttackExecutor> _executors = new();

        private IAttackAnimatorEvents _animatorEvents;
        private AttackDefinition _armedAttack;

        /// <summary>Executors discovered on this character, keyed by the attack kind they serve.</summary>
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

        public bool Supports(AttackKind kind)
        {
            return _executors.ContainsKey(kind);
        }

        /// <summary>Arms the attack whose hit event is about to fire. Called from an attack state's Enter().</summary>
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
