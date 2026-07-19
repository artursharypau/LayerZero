using Characters.Player.States;
using Core.StateMachine;
using Core.Utils;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterController = Characters.Common.CharacterController;

namespace Characters.Player
{
    public sealed class PlayerController : CharacterController
    {
        [Header("Movement details")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] [Range(1, 5)] private float _dashMultiplier = 3f;
        [SerializeField] private float _dashCooldown = 2f;
        [SerializeField] private float _jumpForce = 13f;
        [SerializeField] private ushort _jumpsCount = 2;
        [SerializeField] private Vector2 _wallJumpForce = new(6f, 12f);
        [SerializeField] private float _wallJumpMoveLockDuration = 0.2f;
        [SerializeField] [Range(0, 1)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0, 1)] private float _wallSlideMultiplier = 0.8f;

        [Header("Attack details")]
        [SerializeField] private Vector2[] _attackVelocities = { new(3f, 1.5f), new(1f, 2.5f), new(4f, 5f) };
        [SerializeField] private float _attackVelocityDuration = 0.1f;
        [SerializeField] private float _attackResetTime = 1f;
        [SerializeField] private Vector2 _jumpAttackVelocity = new(3f, -5f);

        private bool _jumpRequested;
        private ushort _availableJumps;
        private readonly float _jumpBufferingDuration = 0.2f;
        private CountdownTimer _jumpBufferingTimer;

        public float MoveSpeed => _moveSpeed;
        public float DashDuration => _dashDuration;
        public float DashMultiplier => _dashMultiplier;
        public float DashCooldown => _dashCooldown;
        public float JumpForce => _jumpForce;
        public Vector2 WallJumpForce => _wallJumpForce;
        public float WallJumpMoveLockDuration => _wallJumpMoveLockDuration;
        public float InAirMoveMultiplier => _inAirMoveMultiplier;
        public float WallSlideMultiplier => _wallSlideMultiplier;

        public Vector2[] AttackVelocities => _attackVelocities;
        public float AttackVelocityDuration => _attackVelocityDuration;
        public float AttackResetTime => _attackResetTime;
        public Vector2 JumpAttackVelocity => _jumpAttackVelocity;

        public PlayerInputSet InputSet { get; private set; }
        public PlayerInputSet.PlayerActions InputActions { get; private set; }

        public State IdleState { get; private set; }
        public State MoveState { get; private set; }
        public State DashState { get; private set; }
        public State JumpState { get; private set; }
        public State FallState { get; private set; }
        public State WallSlideState { get; private set; }
        public State WallJumpState { get; private set; }
        public State AttackState { get; private set; }
        public State JumpAttackState { get; private set; }

        public Vector2 MoveInput { get; private set; }

        protected override void OnAwakened()
        {
            _jumpBufferingTimer = new CountdownTimer();

            InputSet = new PlayerInputSet();
            InputActions = InputSet.Player;

            IdleState = new PlayerIdleState(FSM, this);
            MoveState = new PlayerMoveState(FSM, this);
            DashState = new PlayerDashState(FSM, this);
            JumpState = new PlayerJumpState(FSM, this);
            FallState = new PlayerFallState(FSM, this);
            WallSlideState = new PlayerWallSlideState(FSM, this);
            WallJumpState = new PlayerWallJumpState(FSM, this);
            AttackState = new PlayerAttackState(FSM, this);
            JumpAttackState = new PlayerJumpAttackState(FSM, this);
        }

        protected override void OnEnabled()
        {
            InputActions.Enable();
            InputActions.Movement.performed += OnMovementPerformed;
            InputActions.Movement.canceled += OnMovementCanceled;
            InputActions.Jump.performed += OnJumpPerformed;
        }

        protected override void OnStarted()
        {
            _availableJumps = _jumpsCount;

            FSM.Initialize(IdleState);
        }

        protected override void OnUpdated()
        {
            _jumpBufferingTimer.Tick(Time.deltaTime);
            if (_jumpBufferingTimer.IsExpired)
            {
                _jumpRequested = false;
            }
        }

        protected override void OnDisabled()
        {
            InputActions.Disable();
            InputActions.Movement.performed -= OnMovementPerformed;
            InputActions.Movement.canceled -= OnMovementCanceled;
            InputActions.Jump.performed -= OnJumpPerformed;
        }

        protected override void OnDestroyed()
        {
            InputSet.Dispose();
        }

        public bool CanJump()
        {
            return _jumpRequested && _availableJumps > 0;
        }

        public void ConsumeJump()
        {
            _jumpRequested = false;

            if (_availableJumps > 0)
            {
                --_availableJumps;
            }
        }

        public void ResetJump()
        {
            _availableJumps = _jumpsCount;
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            _jumpRequested = true;
            _jumpBufferingTimer.Start(_jumpBufferingDuration);
        }
    }
}
