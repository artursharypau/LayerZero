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
    public abstract class Character : MonoBehaviour
    {
        public StateMachine StateMachine { get; } = new();

        public CharacterMovement2D Movement { get; private set; }
        public CharacterAnimator Animator { get; private set; }
        public IDamageable Health { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistances DamageResistances { get; private set; }

        public CombatSystem Combat { get; private set; }

        public bool IsDead => Health != null && Health.IsDead;

        private void Awake()
        {
            Movement = this.GetRequired<CharacterMovement2D>();
            Health = this.GetRequired<IDamageable>();
            DamageReceiver = this.GetRequired<IDamageReceiver>();
            Combat = this.GetRequired<CombatSystem>();

            Animator = new CharacterAnimator(
                this.GetRequiredInChildren<Animator>(),
                this.GetRequiredInChildren<IAttackAnimatorEvents>());

            DamageResistances = new DamageResistances();
            DamageReceiver.SetResistances(DamageResistances);

            OnInitialized();
        }

        private void OnEnable()
        {
            Health.Died += HandleDied;
            DamageReceiver.ImpactReceived += HandleImpactReceived;
            DamageReceiver.Damaged += HandleDamaged;

            OnEnabled();
        }

        private void Start()
        {
            OnStarted();
        }

        private void Update()
        {
            StateMachine.Update();

            OnUpdated(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Movement.Refresh();
            StateMachine.FixedUpdate();

            OnFixedUpdated(Time.fixedDeltaTime);
        }

        private void OnDisable()
        {
            DamageReceiver.Damaged -= HandleDamaged;
            DamageReceiver.ImpactReceived -= HandleImpactReceived;
            Health.Died -= HandleDied;

            OnDisabled();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        private void OnDrawGizmos()
        {
            if (Movement)
            {
                Movement.DrawGizmos();
            }

            OnGizmosDrawn();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnEnabled()
        {
        }

        protected virtual void OnUpdated(float deltaTime)
        {
        }

        protected virtual void OnFixedUpdated(float deltaTime)
        {
        }

        protected virtual void OnDisabled()
        {
        }

        protected virtual void OnDestroyed()
        {
        }

        protected virtual void OnGizmosDrawn()
        {
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

        private void HandleDamaged(DamageInfo damageInfo)
        {
            if (IsDead)
            {
                return;
            }

            OnDamaged(damageInfo);
        }

        private void HandleImpactReceived(DamageImpactInfo impact)
        {
            if (IsDead)
            {
                return;
            }

            OnDamageImpactReceived(impact);
        }

        private void HandleDied()
        {
            Movement.Stop();

            if (Combat)
            {
                Combat.enabled = false;
            }

            OnDied();
        }
    }
}
