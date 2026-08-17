using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Movement;
using LayerZero.Combat.Attacks;
using LayerZero.Combat.Damage;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Extensions;
using LayerZero.Core.StateMachine;
using UnityEngine;

namespace LayerZero.Characters.Common
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(DamageReceiver))]
    [RequireComponent(typeof(CombatSystem))]
    public abstract class Character2D : MonoBehaviour
    {
        private AnimatorStateBinder _animatorStateBinder;
        private CharacterMovement2D _movement;

        public StateMachine StateMachine { get; } = new();
        public IMovement2D Movement => _movement;
        public CharacterAnimator Animator { get; private set; }
        public IDamageable Health { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistances DamageResistances { get; private set; }
        public CombatSystem Combat { get; private set; }

        public bool IsDead => Health.IsDead;

        protected virtual void Awake()
        {
            _movement = this.GetRequiredComponent<CharacterMovement2D>();
            Health = this.GetRequiredComponent<IDamageable>();
            DamageReceiver = this.GetRequiredComponent<IDamageReceiver>();
            Combat = this.GetRequiredComponent<CombatSystem>();

            Animator = new CharacterAnimator(
                this.GetRequiredComponentInChildren<Animator>(),
                this.GetRequiredComponentInChildren<AnimatorEvents>());

            DamageResistances = new DamageResistances();
            DamageReceiver.SetResistances(DamageResistances);

            _animatorStateBinder = new AnimatorStateBinder(Animator, StateMachine);
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
            _movement.enabled = false;

            Combat.enabled = false;

            OnDied();

            StateMachine.Stop();
            enabled = false;
        }
    }
}
