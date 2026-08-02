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
        private readonly StateMachineHolder _stateMachineHolder = new();

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
            _stateMachineHolder.Update();
            OnUpdated();
        }

        private void FixedUpdate()
        {
            Movement.Refresh();
            _stateMachineHolder.FixedUpdate();
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
            _stateMachineHolder.Register(state);
        }

        protected void StartStateMachine(int initialId)
        {
            _stateMachineHolder.Start(initialId);
        }

        protected void ChangeState(int id)
        {
            _stateMachineHolder.ChangeState(id);
        }

        protected void ChangeState<TArg>(int id, TArg arg)
        {
            _stateMachineHolder.ChangeState(id, arg);
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
