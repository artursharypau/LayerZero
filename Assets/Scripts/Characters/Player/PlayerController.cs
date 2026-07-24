using Characters.Common;
using Characters.Player.Abilities;
using Characters.Player.States;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Characters.Player
{
    public sealed class PlayerController : CharacterControllerBase
    {
        [Header("Movement details")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] private JumpAbility _jumpAbility = new();
        [SerializeField] private DashAbility _dashAbility = new();
        [SerializeField] [Range(0, 1)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0, 1)] private float _wallSlideMultiplier = 0.8f;

        [Header("Attack details")]
        [SerializeField] private Vector2[] _attackVelocities = { new(3f, 1.5f), new(1f, 2.5f), new(4f, 5f) };
        [SerializeField] private float _attackVelocityDuration = 0.1f;
        [SerializeField] private float _attackResetTime = 1f;
        [SerializeField] private Vector2 _jumpAttackVelocity = new(3f, -5f);

        [SerializeField] private PlayerInputHandler _inputHandler;

        public float MoveSpeed => _moveSpeed;
        public JumpAbility JumpAbility => _jumpAbility;
        public DashAbility DashAbility => _dashAbility;
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
            FSM.Initialize(IdleState);
        }

        protected override void OnUpdated()
        {
            _inputHandler.Tick(Time.deltaTime);
            _dashAbility.Tick(Time.deltaTime);
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
            return _inputHandler.WasJumpPerformed() && _jumpAbility.HasJumpsLeft;
        }

        public void ConsumeJump()
        {
            _inputHandler.ConsumeJump();
            _jumpAbility.Consume();
        }

        public bool CanDash()
        {
            return _dashAbility.IsReady && !IsWalled && _inputHandler.WasDashPerformed();
        }
    }
}
