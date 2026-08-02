using System.Collections.Generic;
using Characters.Common.Movement;
using Characters.Common.States;
using Core.StateMachine;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(Health))]
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        private readonly Dictionary<StateId, State> _states = new(5);
        private readonly StateMachine _stateMachine = new();

        public Animator Anim { get; private set; }
        public IAttackAnimatorEvents AttackAnimatorEvents { get; private set; }

        public Health Health { get; private set; }
        public CharacterMovement2D Movement { get; private set; }

        private void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
            AttackAnimatorEvents = GetComponentInChildren<IAttackAnimatorEvents>();

            Health = GetComponent<Health>();
            Movement = GetComponent<CharacterMovement2D>();

            OnAwakened();
        }

        private void OnEnable()
        {
            Health.Damaged += HandleDamaged;
            Health.Died += HandleDied;

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
            Movement.Tick(Time.fixedDeltaTime);
            _stateMachine.FixedUpdate();

            OnFixedUpdated();
        }

        private void OnDisable()
        {
            Health.Damaged -= HandleDamaged;
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

        public void ChangeState(StateId id)
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

        protected void StartStateMachine(StateId initialId)
        {
            _stateMachine.Start(_states[initialId]);
        }

        protected void RegisterState(StateId id, State state)
        {
            _states[id] = state;
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
            if (damageInfo.Impact != DamageImpactInfo.None)
            {
                OnDamageImpactReceived(damageInfo.Impact);
            }
        }

        private void HandleDied()
        {
            OnDied();
        }
    }
}
