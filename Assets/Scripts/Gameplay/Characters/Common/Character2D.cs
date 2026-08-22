using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common.Animation;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Combat.Damage.Resistance;
using UnityEngine;
using VContainer;

namespace LayerZero.Gameplay.Characters.Common
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(DamageReceiver))]
    [RequireComponent(typeof(CombatSystem))]
    public abstract class Character2D : MonoBehaviour
    {
        private AnimatorStateBinder _animatorStateBinder;
        private CharacterMovement2D _movement;

        public StateMachine StateMachine { get; private set; }
        public IMovement2D Movement => _movement;
        public CharacterAnimator Animator { get; private set; }
        public IDamageable Health { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistances DamageResistances { get; private set; }
        public ICombatSystem Combat { get; private set; }

        public bool IsDead => Health.IsDead;

        [Inject]
        public void Construct(
            AnimatorStateBinder animatorStateBinder,
            CharacterMovement2D movement,
            StateMachine stateMachine,
            CharacterAnimator animator,
            IDamageable health,
            IDamageReceiver damageReceiver,
            IDamageResistances damageResistances,
            ICombatSystem combat)
        {
            _animatorStateBinder = animatorStateBinder;
            _movement = movement;

            StateMachine = stateMachine;
            Animator = animator;
            Health = health;
            DamageReceiver = damageReceiver;
            DamageResistances = damageResistances;
            Combat = combat;
        }

        protected virtual void OnEnable()
        {
            _animatorStateBinder.Bind();

            Health.Died += HandleDied;
            DamageReceiver.ImpactReceived += HandleDamageImpactReceived;
            DamageReceiver.Damaged += OnDamaged;
        }

        protected virtual void OnDisable()
        {
            DamageReceiver.Damaged -= OnDamaged;
            DamageReceiver.ImpactReceived -= HandleDamageImpactReceived;
            Health.Died -= HandleDied;

            _animatorStateBinder.Unbind();
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        protected virtual void FixedUpdate()
        {
            _movement.Refresh();
            StateMachine.FixedUpdate();
        }

        protected virtual void OnDrawGizmos()
        {
            _movement?.DrawGizmos();
        }

        protected virtual void OnDamaged(DamageInfo damageInfo)
        {
        }

        protected virtual void OnDamageImpactReceived(DamageImpactInfo impact)
        {
        }

        protected virtual void OnDied()
        {
        }

        private void HandleDamageImpactReceived(DamageImpactInfo impact)
        {
            if (impact.HasImpact)
            {
                OnDamageImpactReceived(impact);
            }
        }

        private void HandleDied()
        {
            _movement.CancelKnockback();
            _movement.SetVelocity(0f, 0f);

            Combat.Disarm();

            OnDied();

            StateMachine.Stop();
            enabled = false;
        }
    }
}
