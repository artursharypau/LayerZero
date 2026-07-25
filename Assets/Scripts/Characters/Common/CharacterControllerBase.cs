using System.Collections.Generic;
using Characters.Common.Detection;
using Characters.Common.States;
using Core.StateMachine;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common
{
    public abstract class CharacterControllerBase : MonoBehaviour, IMovable, IFacing, IPositioned
    {
        [SerializeField] private GroundWallDetector _groundWallDetector;

        private readonly Dictionary<StateId, State> _states = new(5);
        private readonly StateMachine _stateMachine = new();

        private Rigidbody2D _rb;
        private Health _health;

        public bool IsGrounded => _groundWallDetector.IsGrounded;
        public bool IsWalled => _groundWallDetector.IsWalled;
        public bool IsFalling => _rb.linearVelocityY < 0f && !IsGrounded;
        public float VelocityX => _rb.linearVelocityX;
        public float VelocityY => _rb.linearVelocityY;
        public float GravityScale => _rb.gravityScale;

        public bool IsFacingRight { get; private set; } = true;
        public float FacingDirection => IsFacingRight ? 1f : -1f;

        public Vector2 Position => transform.position;

        public Animator Anim { get; private set; }
        public IAttackAnimationEvents AttackAnimationEvents { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();

            Anim = GetComponentInChildren<Animator>();
            AttackAnimationEvents = GetComponentInChildren<IAttackAnimationEvents>();

            _groundWallDetector.Initialize(this);

            OnAwakened();
        }

        private void OnEnable()
        {
            _health.Damaged += HandleDamaged;
            _health.Died += HandleDied;

            OnEnabled();
        }

        private void Start()
        {
            OnStarted();
        }

        private void Update()
        {
            _groundWallDetector.Tick(Time.deltaTime);
            _stateMachine.Update();

            OnUpdated();
        }

        private void OnDisable()
        {
            _health.Damaged -= HandleDamaged;
            _health.Died -= HandleDied;

            OnDisabled();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        private void OnDrawGizmos()
        {
            _groundWallDetector.DrawGizmos();

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

        public void SetVelocity(float x, float y)
        {
            _rb.linearVelocity = new Vector2(x, y);

            if ((IsFacingRight && x < 0f) || (!IsFacingRight && x > 0f))
            {
                Flip();
            }
        }

        public void SetHorizontalVelocity(float x)
        {
            SetVelocity(x, VelocityY);
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            IsFacingRight = !IsFacingRight;

            _groundWallDetector.Tick(Time.deltaTime);
        }

        public void SetGravityScale(float scale)
        {
            _rb.gravityScale = scale;
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
