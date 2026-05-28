using Common;
using InputSystem;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class PlayerController : ControllerBase
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
        [SerializeField] private Vector2[] _attackVelocity = { new(3f, 1.5f), new(1f, 2.5f), new(4f, 5f) };
        [SerializeField] private float _attackVelocityDuration = 0.1f;
        [SerializeField] private float _attackResetTime = 1f;
        [SerializeField] private Vector2 _jumpAttackVelocity = new(3f, -5f);

        private ushort _availableJumps;

        public float MoveSpeed => _moveSpeed;
        public float DashDuration => _dashDuration;
        public float DashMultiplier => _dashMultiplier;
        public float DashCooldown => _dashCooldown;
        public float JumpForce => _jumpForce;
        public Vector2 WallJumpForce => _wallJumpForce;
        public float WallJumpMoveLockDuration => _wallJumpMoveLockDuration;
        public float InAirMoveMultiplier => _inAirMoveMultiplier;
        public float WallSlideMultiplier => _wallSlideMultiplier;

        public Vector2[] AttackVelocity => _attackVelocity;
        public float AttackVelocityDuration => _attackVelocityDuration;
        public float AttackResetTime => _attackResetTime;
        public Vector2 JumpAttackVelocity => _jumpAttackVelocity;

        public PlayerAnimationTriggers AnimTriggers { get; private set; }
        public PlayerInputSet.PlayerActions InputActions { get; private set; }

        public StateBase IdleState { get; private set; }
        public StateBase MoveState { get; private set; }
        public StateBase DashState { get; private set; }
        public StateBase JumpState { get; private set; }
        public StateBase FallState { get; private set; }
        public StateBase WallSlideState { get; private set; }
        public StateBase WallJumpState { get; private set; }
        public StateBase BasicAttackState { get; private set; }
        public StateBase JumpAttackState { get; private set; }

        public Vector2 MoveInput { get; private set; }

        protected override void OnAwake()
        {
            AnimTriggers = GetComponentInChildren<PlayerAnimationTriggers>();
            InputActions = new PlayerInputSet().Player;

            IdleState = new PlayerIdleState(FSM, this);
            MoveState = new PlayerMoveState(FSM, this);
            DashState = new PlayerDashState(FSM, this);
            JumpState = new PlayerJumpState(FSM, this);
            FallState = new PlayerFallState(FSM, this);
            WallSlideState = new PlayerWallSlideState(FSM, this);
            WallJumpState = new PlayerWallJumpState(FSM, this);
            BasicAttackState = new PlayerBasicAttackState(FSM, this);
            JumpAttackState = new PlayerJumpAttackState(FSM, this);
        }

        protected override void OnStart()
        {
            _availableJumps = _jumpsCount;

            FSM.Initialize(IdleState);
        }

        protected override void OnUpdate()
        {
        }

        private void OnEnable()
        {
            InputActions.Enable();
            InputActions.Movement.performed += OnMovementPerformed;
            InputActions.Movement.canceled += OnMovementCanceled;
        }

        private void OnDisable()
        {
            InputActions.Disable();
            InputActions.Movement.performed -= OnMovementPerformed;
            InputActions.Movement.canceled -= OnMovementCanceled;
        }

        public bool CanJump()
        {
            return _availableJumps > 0 && InputActions.Jump.WasPerformedThisFrame();
        }

        public void ConsumeJump()
        {
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
    }
}
