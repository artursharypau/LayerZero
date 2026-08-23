using System;
using LayerZero.Core.Extensions;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common.Collisions;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class CharacterMovement2D : MonoBehaviour, IMovement2D
    {
        [SerializeField] private GroundWallDetector _groundWallDetector = new();
        [SerializeField] [Min(1f)] private float _knockbackDeceleration = 40f;

        private Countdown _knockbackTimer;
        private Rigidbody2D _rigidbody;

        public event Action<float> FacingDirectionChanged;

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

            FacingDirection = transform.right.x < 0f ? -1f : 1f;
        }

        public void Refresh()
        {
            _groundWallDetector.Refresh();
        }

        public void DrawGizmos()
        {
            _groundWallDetector.DrawGizmos();
        }

        public void SetVelocity(float x, float y, bool updateFacing = false)
        {
            if (!_knockbackTimer.IsExpired)
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
            if (!_knockbackTimer.IsExpired)
            {
                return;
            }

            transform.Rotate(0f, 180f, 0f);
            FacingDirection = -FacingDirection;

            FacingDirectionChanged?.Invoke(FacingDirection);

            _groundWallDetector.Refresh();
        }

        public void SetGravityScale(float scale)
        {
            _rigidbody.gravityScale = scale;
        }

        public void ApplyKnockback(Vector2 knockback)
        {
            if (knockback == Vector2.zero)
            {
                return;
            }

            _rigidbody.linearVelocity = knockback;

            float duration = Mathf.Abs(knockback.x) / _knockbackDeceleration;
            _knockbackTimer.Start(duration);
        }

        public void CancelKnockback()
        {
            _knockbackTimer.Stop();
        }
    }
}
