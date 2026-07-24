using Infrastructure.Animation;
using Infrastructure.StateMachine;
using Systems.Combat;
using UnityEngine;

namespace Characters.Common
{
    public abstract class CharacterControllerBase : MonoBehaviour, IMovable
    {
        [SerializeField] private GroundWallDetector _groundWallDetector;

        public bool IsGrounded => _groundWallDetector.IsGrounded;
        public bool IsWalled => _groundWallDetector.IsWalled;
        public bool IsFalling => RB.linearVelocityY < 0f && !IsGrounded;
        public bool IsFacingRight { get; private set; } = true;
        public float FacingDirection => IsFacingRight ? 1f : -1f;

        public Rigidbody2D RB { get; private set; }
        public Animator Anim { get; private set; }
        public AnimatorTriggers AnimTriggers { get; private set; }
        public Health Health { get; private set; }
        public StateMachine FSM { get; private set; }

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponentInChildren<Animator>();
            AnimTriggers = GetComponentInChildren<AnimatorTriggers>();
            Health = GetComponent<Health>();

            FSM = new StateMachine();

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

            OnUpdated();
            FSM.Update();
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

            OnDrownGizmos();
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

        protected virtual void OnDrownGizmos()
        {
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
