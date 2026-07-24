using System.Collections.Generic;
using System.Linq;
using Characters.Common;
using Characters.Player.Abilities;
using Characters.Player.Abilities.Dash;
using Characters.Player.Abilities.Jump;
using Characters.Player.Input;
using Characters.Player.States;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Characters.Player
{
    public sealed class PlayerController : CharacterControllerBase
    {
        [Header("Movement details")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] private PlayerJumpAbility _jumpAbility;
        [SerializeField] private PlayerDashAbility _dashAbility;
        [SerializeField] [Range(0, 1)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0, 1)] private float _wallSlideMultiplier = 0.8f;

        [Header("Attack details")]
        [SerializeField] private Vector2[] _attackVelocities = { new(3f, 1.5f), new(1f, 2.5f), new(4f, 5f) };
        [SerializeField] private float _attackVelocityDuration = 0.1f;
        [SerializeField] private float _attackResetTime = 1f;
        [SerializeField] private Vector2 _jumpAttackVelocity = new(3f, -5f);

        [SerializeField] private PlayerInputHandler _inputHandler;

        private PlayerAbilityContext _abilityContext;
        private Dictionary<PlayerAbilityId, IPlayerAbility> _abilities;
        private IPlayerTickableAbility[] _tickableAbilities;

        public float MoveSpeed => _moveSpeed;
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
            _inputHandler.Initialize();
            _abilityContext = new PlayerAbilityContext(this, _inputHandler);
            _abilities = new Dictionary<PlayerAbilityId, IPlayerAbility>
            {
                { PlayerAbilityId.Jump, _jumpAbility },
                { PlayerAbilityId.Dash, _dashAbility }
            };
            _tickableAbilities = _abilities.Values.OfType<IPlayerTickableAbility>().ToArray();

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
            _inputHandler.Enable();
        }

        protected override void OnStarted()
        {
            RefillChargeableAbility(PlayerAbilityId.Jump);
            FSM.Initialize(IdleState);
        }

        protected override void OnUpdated()
        {
            _inputHandler.Tick(Time.deltaTime);

            foreach (IPlayerTickableAbility ability in _tickableAbilities)
            {
                ability.Tick(Time.deltaTime);
            }
        }

        protected override void OnDisabled()
        {
            _inputHandler.Disable();
        }

        protected override void OnDestroyed()
        {
            _inputHandler.Dispose();
        }

        public bool TryGetAbilityConfig<TConfig>(PlayerAbilityId id, out TConfig config)
            where TConfig : class, IPlayerAbilityConfig
        {
            config = null;
            bool result = false;

            if (TryGetAbility(id, out IPlayerAbility ability) && ability.GetConfig() is TConfig typedConfig)
            {
                config = typedConfig;
                result = true;
            }
            else
            {
                Debug.unityLogger.LogError(
                    $"{nameof(PlayerController)}.{nameof(TryGetAbilityConfig)}",
                    $"Ability '{id}' has no config of type '{typeof(TConfig).Name}'");
            }

            return result;
        }

        public bool CanUseAbility(PlayerAbilityId id)
        {
            return TryGetAbility(id, out IPlayerAbility ability) && ability.CanBeUsed(_abilityContext);
        }

        public void TriggerAbility(PlayerAbilityId id)
        {
            if (TryGetAbility(id, out IPlayerAbility ability))
            {
                ability.Trigger(_abilityContext);
            }
        }

        public void RefillChargeableAbility(PlayerAbilityId id)
        {
            if (TryGetAbility(id, out IPlayerAbility ability) && ability is IPlayerChargeableAbility chargeableAbility)
            {
                chargeableAbility.Refill();
            }
        }

        private bool TryGetAbility(PlayerAbilityId id, out IPlayerAbility ability)
        {
            if (_abilities.TryGetValue(id, out ability))
            {
                return true;
            }

            Debug.unityLogger.LogError($"{nameof(PlayerController)}.{nameof(TryGetAbility)}", $"Ability '{id}' is not registered");
            return false;
        }
    }
}
