using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Characters.Common.Movement
{
    /// <summary>
    /// Rigidbody wrapper shared by every character. Owns velocity, facing and contact
    /// detection; contains no decisions - those belong to states.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class CharacterMovement2D : MonoBehaviour, IMovable
    {
        [SerializeField] private GroundWallDetector _groundWallDetector = new();

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
            _rigidbody = this.GetRequired<Rigidbody2D>();
            _groundWallDetector.Initialize(this);
        }

        /// <summary>Re-samples ground/wall contacts. Driven once per physics step by the character.</summary>
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

        public void Stop()
        {
            SetVelocity(0f, 0f);
        }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            FacingDirection = -FacingDirection;

            // Wall contacts are direction dependent, so they are stale the moment we turn.
            _groundWallDetector.Refresh();
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

        public void SetGravityScale(float scale)
        {
            _rigidbody.gravityScale = scale;
        }
    }
}
