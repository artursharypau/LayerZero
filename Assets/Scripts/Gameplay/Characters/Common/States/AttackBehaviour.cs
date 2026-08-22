using System;
using LayerZero.Gameplay.Combat.Attack;

namespace LayerZero.Gameplay.Characters.Common.States
{
    public sealed class AttackBehaviour
    {
        private readonly Character2D _owner;
        private readonly Func<AttackDefinition> _resolveAttack;

        public AttackBehaviour(Character2D owner, Func<AttackDefinition> resolveAttack)
        {
            _owner = owner;
            _resolveAttack = resolveAttack;
        }

        public bool IsFinished { get; private set; }

        public void Begin()
        {
            IsFinished = false;

            AttackDefinition attack = _resolveAttack();
            if (attack != null)
            {
                _owner.Combat.Arm(attack);
            }

            _owner.Animator.Events.AttackFinished += OnAttackFinished;
        }

        public void End()
        {
            IsFinished = true;

            _owner.Animator.Events.AttackFinished -= OnAttackFinished;
            _owner.Combat.Disarm();
        }

        private void OnAttackFinished()
        {
            IsFinished = true;

            _owner.Animator.Events.AttackFinished -= OnAttackFinished;
        }
    }
}
