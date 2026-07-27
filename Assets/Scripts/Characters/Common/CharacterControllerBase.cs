using System.Collections.Generic;
using Characters.Common.Detection;
using Characters.Common.States;
using Core.StateMachine;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Common
{
    public abstract class CharacterControllerBase : MonoBehaviour, IMovable, IPositioned
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

        public float FacingDirection { get; private set; } = 1f;
        public Vector2 Position => transform.position;

        public Animator Anim { get; private set; }
        public IAttackAnimatorEvents AttackAnimatorEvents { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();

            Anim = GetComponentInChildren<Animator>();
            AttackAnimatorEvents = GetComponentInChildren<IAttackAnimatorEvents>();

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
            _stateMachine.Update();

            OnUpdated();
        }

        private void FixedUpdate()
        {
            _groundWallDetector.Tick(Time.fixedDeltaTime);
            _stateMachine.FixedUpdate();

            OnFixedUpdated();
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

        public void SetVelocityX(float x, bool updateFacing = false)
        {
            SetVelocity(x, VelocityY, updateFacing);
        }

        public void SetVelocity(float x, float y, bool updateFacing = false)
        {
            _rb.linearVelocity = new Vector2(x, y);
            if (updateFacing)
            {
                TryFaceTowards(x);
            }
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            FacingDirection = -FacingDirection;

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

        private void TryFaceTowards(float x)
        {
            if ((FacingDirection > 0f && x < 0f) || (FacingDirection < 0f && x > 0f))
            {
                Flip();
            }
        }
    }
}
