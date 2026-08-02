using System.Collections.Generic;
using System.Linq;
using Characters.Common;
using Characters.Player.Abilities;
using Characters.Player.Abilities.Chargeable;
using Characters.Player.Abilities.Config;
using Characters.Player.Abilities.Tickable;
using Characters.Player.Input;
using Characters.Player.States;
using UnityEngine;

namespace Characters.Player
{
    public sealed class PlayerController : CharacterControllerBase<PlayerStateId>
    {
        [Header("Movement details")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] [Range(0, 1)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0, 1)] private float _wallSlideMultiplier = 0.8f;
        [SerializeField] private PlayerJumpAbilityConfig _jumpConfig;
        [SerializeField] private PlayerDashAbilityConfig _dashConfig;

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

        public bool CanUseAbility(PlayerAbilityId id)
        {
            return TryGetAbility(id, out IPlayerAbility ability) && ability.CanBeUsed(_abilityContext);
        }

        public bool TryTriggerAbility(PlayerAbilityId id)
        {
            if (TryGetAbility(id, out IPlayerAbility ability) && ability.CanBeUsed(_abilityContext))
            {
                ability.Trigger(_abilityContext);
                return true;
            }

            return false;
        }

        public void RefillChargeableAbility(PlayerAbilityId id, int amount = -1)
        {
            if (TryGetAbility(id, out IPlayerAbility ability) && ability is IPlayerChargeableAbility chargeableAbility)
            {
                chargeableAbility.Refill(amount);
            }
        }

        protected override void OnAwakened()
        {
            _inputHandler.Initialize();
            _abilityContext = new PlayerAbilityContext(Movement, _inputHandler);
            _abilities = new Dictionary<PlayerAbilityId, IPlayerAbility>
            {
                { PlayerAbilityId.Jump, new PlayerJumpAbility(_jumpConfig) },
                { PlayerAbilityId.Dash, new PlayerDashAbility(_dashConfig) }
            };
            _tickableAbilities = _abilities.Values.OfType<IPlayerTickableAbility>().ToArray();

            RegisterState(PlayerStateId.Idle, new PlayerIdleState(this));
            RegisterState(PlayerStateId.Move, new PlayerMoveState(this));
            RegisterState(PlayerStateId.Dash, new PlayerDashState(this, _dashConfig));
            RegisterState(PlayerStateId.Jump, new PlayerJumpState(this, _jumpConfig));
            RegisterState(PlayerStateId.Fall, new PlayerFallState(this));
            RegisterState(PlayerStateId.WallSlide, new PlayerWallSlideState(this));
            RegisterState(PlayerStateId.WallJump, new PlayerWallJumpState(this, _jumpConfig));
            RegisterState(PlayerStateId.Attack, new PlayerAttackState(this));
            RegisterState(PlayerStateId.JumpAttack, new PlayerJumpAttackState(this));
        }

        protected override void OnEnabled()
        {
            _inputHandler.Enable();
        }

        protected override void OnStarted()
        {
            StartStateMachine(PlayerStateId.Idle);
            RefillChargeableAbility(PlayerAbilityId.Jump);
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
