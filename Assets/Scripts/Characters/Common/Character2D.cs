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
        public StateMachine StateMachine { get; } = new();
        public CharacterMovement2D Movement { get; private set; }
        public CharacterAnimator Animator { get; private set; }
        public IDamageable Health { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistances DamageResistances { get; private set; }
        public CombatSystem Combat { get; private set; }

        public bool IsDead => Health.IsDead;

        protected virtual void Awake()
        {
            Movement = this.GetRequiredComponent<CharacterMovement2D>();
            Health = this.GetRequiredComponent<IDamageable>();
            DamageReceiver = this.GetRequiredComponent<IDamageReceiver>();
            Combat = this.GetRequiredComponent<CombatSystem>();

            Animator = new CharacterAnimator(
                this.GetRequiredComponentInChildren<Animator>(),
                this.GetRequiredComponentInChildren<AnimatorEvents>());

            DamageResistances = new DamageResistances();
            DamageReceiver.SetResistances(DamageResistances);
        }

        protected virtual void OnEnable()
        {
            Health.Died += HandleDied;
            DamageReceiver.ImpactReceived += OnDamageImpactReceived;
            DamageReceiver.Damaged += OnDamaged;
        }

        protected virtual void OnDisable()
        {
            DamageReceiver.Damaged -= OnDamaged;
            DamageReceiver.ImpactReceived -= OnDamageImpactReceived;
            Health.Died -= HandleDied;
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        protected virtual void FixedUpdate()
        {
            Movement.Refresh();
            StateMachine.FixedUpdate();
        }

        protected virtual void OnDrawGizmos()
        {
            Movement?.DrawGizmos();
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

        private void HandleDied()
        {
            Movement.SetVelocity(0f, 0f);
            Combat.enabled = false;

            OnDied();
        }
    }
}
