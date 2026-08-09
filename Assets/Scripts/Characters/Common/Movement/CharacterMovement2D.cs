using LayerZero.Characters.Common.Collisions;
using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Common.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class CharacterMovement2D : MonoBehaviour, IMovement2D
    {
        [SerializeField] private GroundWallDetector _groundWallDetector = new();

        private Countdown _lockTimer;
        private Rigidbody2D _rigidbody;

        public bool IsGrounded => _groundWallDetector.IsGrounded;
        public bool IsWalled => _groundWallDetector.IsWalled;
        public bool IsFalling => !IsGrounded && _rigidbody.linearVelocityY < 0f;

        public float VelocityX => _rigidbody.linearVelocityX;
        public float VelocityY => _rigidbody.linearVelocityY;
        public float GravityScale => _rigidbody.gravityScale;

        public float FacingDirection { get; private set; } = 1f;
        public Vector2 Position => transform.position;
        public Vector2 FacingVector => new(FacingDirection, 0f);

        private void Awake()
        {
            _rigidbody = this.GetRequiredComponent<Rigidbody2D>();
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

        public void LockVelocityFor(float duration)
        {
            _lockTimer.Start(duration);
        }

        public void SetVelocity(float x, float y, bool updateFacing = false)
        {
            if (!_lockTimer.IsExpired)
            {
                return;
            }

            _rigidbody.linearVelocity = new Vector2(x, y);

            if (updateFacing)
            {
                FaceTowards(x);
            }
        }

        public void SetVelocityX(float x, bool updateFacing = false)
        {
            SetVelocity(x, VelocityY, updateFacing);
        }

        public void FaceTowards(float direction)
        {
            if (!_lockTimer.IsExpired)
            {
                return;
            }

            if (direction == 0f)
            {
                return;
            }

            if ((FacingDirection > 0f && direction < 0f) || (FacingDirection < 0f && direction > 0f))
            {
                Flip();
            }
        }

        public void Flip()
        {
            if (!_lockTimer.IsExpired)
            {
                return;
            }

            transform.Rotate(0f, 180f, 0f);
            FacingDirection = -FacingDirection;

            _groundWallDetector.Refresh();
        }

        public void SetGravityScale(float scale)
        {
            _rigidbody.gravityScale = scale;
        }
    }
}
