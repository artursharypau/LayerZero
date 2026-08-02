using System;
using System.Collections.Generic;
using Characters.Common.Movement;
using Core.StateMachine;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(DamageReceiver))]
    public abstract class CharacterController2D : MonoBehaviour
    {
        public Health Health { get; private set; }
        public DamageReceiver DamageReceiver { get; private set; }
        public CharacterMovement2D Movement { get; private set; }
        public CombatSystem Combat { get; private set; }

        public Animator Anim { get; private set; }
        public IAttackAnimatorEvents AttackAnimatorEvents { get; private set; }

        protected StateMachine StateMachine { get; private set; }

        private void Awake()
        {
            Health = GetComponent<Health>();
            DamageReceiver = GetComponent<DamageReceiver>();
            Movement = GetComponent<CharacterMovement2D>();
            Combat = GetComponent<CombatSystem>();

            Anim = GetComponentInChildren<Animator>();
            AttackAnimatorEvents = GetComponentInChildren<IAttackAnimatorEvents>();

            StateMachine = new StateMachine();

            OnAwakened();
        }

        private void OnEnable()
        {
            Health.Died += HandleDied;
            DamageReceiver.DamageImpactReceived += HandleDamageImpactReceived;

            OnEnabled();
        }

        private void Start()
        {
            OnStarted();
        }

        private void Update()
        {
            StateMachine.Update();

            OnUpdated();
        }

        private void FixedUpdate()
        {
            Movement.Refresh();
            StateMachine.FixedUpdate();

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

    public abstract class CharacterController2D<TStateId> : CharacterController2D
        where TStateId : struct, Enum
    {
        private readonly Dictionary<TStateId, State> _states = new(4);

        public void ChangeState(TStateId id)
        {
            if (_states.TryGetValue(id, out State state))
            {
                StateMachine.ChangeState(state);
            }
            else
            {
                throw new KeyNotFoundException($"State with id {id} was not found");
            }
        }

        protected void RegisterState(TStateId id, State state)
        {
            _states[id] = state;
        }

        protected void StartStateMachine(TStateId initialId)
        {
            StateMachine.Start(_states[initialId]);
        }
    }
}
