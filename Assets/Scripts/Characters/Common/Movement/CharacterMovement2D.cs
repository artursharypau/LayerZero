using Characters.Common.Detection;
using UnityEngine;

namespace Characters.Common.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement2D : MonoBehaviour, IMovable, IPositioned
    {
        [SerializeField] private GroundWallDetector _groundWallDetector;

        private Rigidbody2D _rb;

        public bool IsGrounded => _groundWallDetector.IsGrounded;
        public bool IsWalled => _groundWallDetector.IsWalled;
        public bool IsFalling => _rb.linearVelocityY < 0f && !IsGrounded;
        public float VelocityX => _rb.linearVelocityX;
        public float VelocityY => _rb.linearVelocityY;
        public float GravityScale => _rb.gravityScale;

        public float FacingDirection { get; private set; } = 1f;
        public Vector2 Position => transform.position;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundWallDetector.Initialize(this);
        }

        public void Refresh()
        {
            _groundWallDetector.Refresh();
        }

        public void DrawGizmos()
        {
            _groundWallDetector.DrawGizmos();
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

            _groundWallDetector.Refresh();
        }

        public void SetGravityScale(float scale)
        {
            _rb.gravityScale = scale;
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
