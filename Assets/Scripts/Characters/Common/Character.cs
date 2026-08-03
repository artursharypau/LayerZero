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
    /// <summary>
    /// Composition root shared by every character. It wires the components, drives the module
    /// list and the state machine, and routes damage/death into the state machine.
    /// <para>
    /// Subclasses do exactly two things: declare their modules and states in <see cref="Compose" />,
    /// and say which state to start in / react to. All behaviour lives in modules and states.
    /// </para>
    /// </summary>
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(DamageReceiver))]
    public abstract class Character : MonoBehaviour
    {
        private readonly List<CharacterModule> _modules = new();

        public StateMachine States { get; } = new();

        public CharacterMovement2D Movement { get; private set; }
        public CharacterAnimator Animation { get; private set; }
        public IDamageable Health { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistances Resistances { get; private set; }

        /// <summary>May be null: not every character fights.</summary>
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

            Resistances = new DamageResistances();
            DamageReceiver.SetResistances(Resistances);

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

            States.Update();
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            Movement.Refresh();

            foreach (CharacterModule module in _modules)
            {
                module.FixedTick(deltaTime);
            }

            States.FixedUpdate();
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

        /// <summary>Registers this character's modules and states. Called once, during Awake.</summary>
        protected abstract void Compose();

        /// <summary>Entry point for the state machine. Called once, during Start.</summary>
        protected abstract void OnStarted();

        protected virtual void OnDamaged(DamageInfo damageInfo)
        {
        }

        /// <summary>A hit that carries knockback or stun landed - usually a transition into a hurt state.</summary>
        protected virtual void OnImpactReceived(DamageImpactInfo impact)
        {
        }

        protected virtual void OnDied()
        {
        }

        /// <summary>Registers and initializes a module. Safe to call after Awake as well.</summary>
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
