using System.Collections.Generic;
using Characters.Common.Movement;
using Core.StateMachine;
using Systems.Combat;
using Systems.Damage;
using Systems.Damage.Resistance;
using UnityEngine;

namespace Characters.Common
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CombatSystem))]
    [RequireComponent(typeof(IDamageReceiver))]
    public abstract class CharacterController2D : MonoBehaviour
    {
        private readonly StateMachine _stateMachine = new();
        private readonly Dictionary<int, State> _states = new();

        public CharacterMovement2D Movement { get; private set; }
        public Health Health { get; private set; }
        public CombatSystem Combat { get; private set; }
        public IDamageReceiver DamageReceiver { get; private set; }
        public IDamageResistanceApplier DamageResistanceApplier { get; private set; }

        public Animator Anim { get; private set; }
        public IAttackAnimatorEvents AttackAnimatorEvents { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<CharacterMovement2D>();
            Health = GetComponent<Health>();
            Combat = GetComponent<CombatSystem>();
            DamageReceiver = GetComponent<IDamageReceiver>();
            DamageResistanceApplier = new DamageResistanceApplier();

            Anim = GetComponentInChildren<Animator>();
            AttackAnimatorEvents = GetComponentInChildren<IAttackAnimatorEvents>();

            DamageReceiver.SetDamageResistanceApplier(DamageResistanceApplier);

            OnAwakened();
        }

        private void OnEnable()
        {
            Health.Died += HandleDied;
            DamageReceiver.Damaged += HandleDamaged;
            DamageReceiver.DamageImpactReceived += HandleDamageImpactReceived;

            OnEnabled();
        }

        private void Start()
        {
            OnStarted();
        }

        private void Update()
        {
            _stateMachine.Update();

            OnUpdated();
        }

        private void FixedUpdate()
        {
            Movement.Refresh();
            _stateMachine.FixedUpdate();

            OnFixedUpdated();
        }

        private void OnDisable()
        {
            DamageReceiver.Damaged -= HandleDamaged;
            DamageReceiver.DamageImpactReceived -= HandleDamageImpactReceived;
            Health.Died -= HandleDied;

            OnDisabled();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        private void OnDrawGizmos()
        {
            if (Movement != null)
            {
                Movement.DrawGizmos();
            }

            OnGizmosDrawn();
        }

        protected virtual void OnAwakened()
        {
        }

        protected virtual void OnEnabled()
        {
        }

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnUpdated()
        {
        }

        protected virtual void OnFixedUpdated()
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

        protected virtual void OnDamageImpactReceived(DamageImpactInfo damageImpact)
        {
        }

        protected virtual void OnDied()
        {
        }

        protected void RegisterState(State state)
        {
            _states[state.Id] = state;
        }

        protected void StartStateMachine(int initialId)
        {
            if (!_states.TryGetValue(initialId, out State state))
            {
                throw new KeyNotFoundException($"State with id {initialId} was not found");
            }

            _stateMachine.Start(state);
        }

        protected void ChangeState(int id)
        {
            if (_states.TryGetValue(id, out State state))
            {
                _stateMachine.ChangeState(state);
            }
            else
            {
                throw new KeyNotFoundException($"State with id {id} was not found");
            }
        }

        private void HandleDamaged(DamageInfo damageInfo)
        {
            OnDamaged(damageInfo);
        }

        private void HandleDamageImpactReceived(DamageImpactInfo damageImpact)
        {
            OnDamageImpactReceived(damageImpact);
        }

        private void HandleDied()
        {
            OnDied();
        }
    }
}
