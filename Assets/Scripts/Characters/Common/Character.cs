using System.Collections.Generic;
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
        private readonly List<CharacterModule> _modules = new();

        public StateMachine StateMachine { get; } = new();

        public CharacterMovement2D Movement { get; private set; }
        public CharacterAnimator Animation { get; private set; }
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
            Combat = GetComponent<CombatSystem>();

            Animation = new CharacterAnimator(
                GetComponentInChildren<Animator>(true),
                GetComponentInChildren<IAttackAnimatorEvents>(true));

            DamageResistances = new DamageResistances();
            DamageReceiver.SetResistances(DamageResistances);

            Compose();
        }

        private void OnEnable()
        {
            Health.Died += HandleDied;
            DamageReceiver.ImpactReceived += HandleImpactReceived;
            DamageReceiver.Damaged += HandleDamaged;

            foreach (CharacterModule module in _modules)
            {
                module.Enable();
            }
        }

        private void Start()
        {
            OnStarted();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            foreach (CharacterModule module in _modules)
            {
                module.Tick(deltaTime);
            }

            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            Movement.Refresh();

            foreach (CharacterModule module in _modules)
            {
                module.FixedTick(deltaTime);
            }

            StateMachine.FixedUpdate();
        }

        private void OnDisable()
        {
            DamageReceiver.Damaged -= HandleDamaged;
            DamageReceiver.ImpactReceived -= HandleImpactReceived;
            Health.Died -= HandleDied;

            foreach (CharacterModule module in _modules)
            {
                module.Disable();
            }
        }

        private void OnDestroy()
        {
            foreach (CharacterModule module in _modules)
            {
                module.Dispose();
            }

            _modules.Clear();
        }

        private void OnDrawGizmos()
        {
            if (Movement)
            {
                Movement.DrawGizmos();
            }

            foreach (CharacterModule module in _modules)
            {
                module.DrawGizmos();
            }
        }

        protected abstract void Compose();

        protected abstract void OnStarted();

        protected virtual void OnDamaged(DamageInfo damageInfo)
        {
        }

        protected virtual void OnImpactReceived(DamageImpactInfo impact)
        {
        }

        protected virtual void OnDied()
        {
        }

        protected TModule AddModule<TModule>(TModule module) where TModule : CharacterModule
        {
            _modules.Add(module);
            module.Bind(this);
            return module;
        }

        private void HandleDamaged(DamageInfo damageInfo)
        {
            OnDamaged(damageInfo);
        }

        private void HandleImpactReceived(DamageImpactInfo impact)
        {
            if (IsDead)
            {
                return;
            }

            OnImpactReceived(impact);
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
