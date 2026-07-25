using System.Collections.Generic;
using Characters.Common.Combat;
using Characters.Common.Detection;
using Core.StateMachine;
using UnityEngine;

namespace Characters.Common
{
    public abstract class CharacterControllerBase : MonoBehaviour, IMovable, IFacing, IPositioned
    {
        [SerializeField] private GroundWallDetector _groundWallDetector;

        private readonly Dictionary<StateId, State> _states = new(5);
        private readonly StateMachine _stateMachine = new();

        public bool IsGrounded => _groundWallDetector.IsGrounded;
        public bool IsWalled => _groundWallDetector.IsWalled;
        public bool IsFalling => RB.linearVelocityY < 0f && !IsGrounded;
        public bool IsFacingRight { get; private set; } = true;
        public float FacingDirection => IsFacingRight ? 1f : -1f;
        public Vector2 Position => transform.position;

        public Rigidbody2D RB { get; private set; }
        public Animator Anim { get; private set; }
        public IAttackFeedback AttackFeedback { get; private set; }
        public Health Health { get; private set; }

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponentInChildren<Animator>();
            AttackFeedback = GetComponentInChildren<IAttackFeedback>();
            Health = GetComponent<Health>();

            _groundWallDetector.Initialize(this);

            OnAwakened();
        }

        private void OnEnable()
        {
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
            RB.linearVelocity = new Vector2(x, y);

            if ((IsFacingRight && x < 0f) || (!IsFacingRight && x > 0f))
            {
                Flip();
            }
        }

        public void SetHorizontalVelocity(float x)
        {
            SetVelocity(x, RB.linearVelocityY);
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            IsFacingRight = !IsFacingRight;

            _groundWallDetector.Tick(Time.deltaTime);
        }
    }
}
