using UnityEngine;

namespace Common
{
    public abstract class ControllerBase : MonoBehaviour
    {
        [Header("Collision detection")]
        [SerializeField] private float _groundCheckDistance = 1.35f;
        [SerializeField] private float _wallCheckDistance = 0.5f;
        [SerializeField] private Transform _upWallCheckTransform;
        [SerializeField] private Transform _downWallCheckTransform;

        public bool IsGrounded { get; private set; } = true;
        public bool IsFalling => RB.linearVelocityY < 0f && !IsGrounded;
        public bool IsWalled { get; private set; }
        public bool IsFacingRight { get; private set; } = true;
        public float FacingDirection => IsFacingRight ? 1f : -1f;

        public Rigidbody2D RB { get; private set; }
        public Animator Anim { get; private set; }
        public StateMachine FSM { get; private set; }

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponentInChildren<Animator>();

            FSM = new StateMachine();

            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            HandleCollisionDetection();
            FSM.Update();

            OnUpdate();
        }

        protected abstract void OnAwake();
        protected abstract void OnStart();
        protected abstract void OnUpdate();

        public void SetVelocity(float x, float y)
        {
            RB.linearVelocity = new Vector2(x, y);

            if ((IsFacingRight && x < 0f) || (!IsFacingRight && x > 0f))
            {
                Flip();
            }
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            IsFacingRight = !IsFacingRight;
        }

        private void HandleCollisionDetection()
        {
            Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;

            IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMaskProvider.Ground);
            IsWalled = Physics2D.Raycast(_upWallCheckTransform.position, direction, _wallCheckDistance, LayerMaskProvider.Ground)
                       && Physics2D.Raycast(_downWallCheckTransform.position, direction, _wallCheckDistance, LayerMaskProvider.Ground);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, -_groundCheckDistance));
            Gizmos.DrawLine(_upWallCheckTransform.position, _upWallCheckTransform.position + new Vector3(_wallCheckDistance * FacingDirection, 0f));
            Gizmos.DrawLine(
                _downWallCheckTransform.position, _downWallCheckTransform.position + new Vector3(_wallCheckDistance * FacingDirection, 0f));
        }
    }
}
