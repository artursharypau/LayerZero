using Characters.Player.States;
using Core.StateMachine;
using UnityEngine;
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

        [SerializeField] private PlayerInputHandler _inputHandler;

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

        public Vector2[] AttackVelocities => _attackVelocities;
        public float AttackVelocityDuration => _attackVelocityDuration;
        public float AttackResetTime => _attackResetTime;
        public Vector2 JumpAttackVelocity => _jumpAttackVelocity;

        public PlayerInputHandler InputHandler => _inputHandler;

        public State IdleState { get; private set; }
        public State MoveState { get; private set; }
        public State DashState { get; private set; }
        public State JumpState { get; private set; }
        public State FallState { get; private set; }
        public State WallSlideState { get; private set; }
        public State WallJumpState { get; private set; }
        public State AttackState { get; private set; }
        public State JumpAttackState { get; private set; }

        protected override void OnAwakened()
        {
            IdleState = new PlayerIdleState(FSM, this);
            MoveState = new PlayerMoveState(FSM, this);
            DashState = new PlayerDashState(FSM, this);
            JumpState = new PlayerJumpState(FSM, this);
            FallState = new PlayerFallState(FSM, this);
            WallSlideState = new PlayerWallSlideState(FSM, this);
            WallJumpState = new PlayerWallJumpState(FSM, this);
            AttackState = new PlayerAttackState(FSM, this);
            JumpAttackState = new PlayerJumpAttackState(FSM, this);

            _inputHandler.Initialize();
        }

        protected override void OnEnabled()
        {
            _inputHandler.Enable();
        }

        protected override void OnStarted()
        {
            _availableJumps = _jumpsCount;

            FSM.Initialize(IdleState);
        }

        protected override void OnUpdated()
        {
            _inputHandler.Tick(Time.deltaTime);
        }

        protected override void OnDisabled()
        {
            _inputHandler.Disable();
        }

        protected override void OnDestroyed()
        {
            _inputHandler.Dispose();
        }

        public bool CanJump()
        {
            return _inputHandler.WasJumpPerformed() && _availableJumps > 0;
        }

        public void ConsumeJump()
        {
            _inputHandler.ConsumeJump();

            if (_availableJumps > 0)
            {
                --_availableJumps;
            }
        }

        public void ResetJump()
        {
            _availableJumps = _jumpsCount;
        }
    }
}
