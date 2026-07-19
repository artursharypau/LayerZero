using Core.Animation;
using Core.StateMachine;
using Core.Utils;
using Systems.Combat;
using UnityEngine;

namespace Characters.Common
{
    public abstract class CharacterController : MonoBehaviour, IMovable
    {
        [Header("Collision detection")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckDistance = 1.35f;
        [SerializeField] private Transform[] _wallCheckPoints;
        [SerializeField] private float _wallCheckDistance = 0.5f;

        public bool IsGrounded { get; private set; } = true;
        public bool IsFalling => RB.linearVelocityY < 0f && !IsGrounded;
        public bool IsWalled { get; private set; }
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
            HandleCollisionDetection();
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
            Gizmos.DrawLine(_groundCheckPoint.position, _groundCheckPoint.position + new Vector3(0f, -_groundCheckDistance));
            foreach (Transform wallCheckPoint in _wallCheckPoints)
            {
                Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + new Vector3(_wallCheckDistance * FacingDirection, 0f));
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

        protected virtual void OnDisabled()
        {
        }

        protected virtual void OnDestroyed()
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

            HandleCollisionDetection();
        }

        private void HandleCollisionDetection()
        {
            IsGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance, LayerMaskProvider.Ground);
            IsWalled = true;

            Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;
            foreach (Transform wallCheckPoint in _wallCheckPoints)
            {
                if (!Physics2D.Raycast(wallCheckPoint.position, direction, _wallCheckDistance, LayerMaskProvider.Ground))
                {
                    IsWalled = false;
                    break;
                }
            }
        }
    }
}
