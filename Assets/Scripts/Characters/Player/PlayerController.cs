using System.Collections.Generic;
using System.Linq;
using Characters.Common;
using Characters.Player.Abilities;
using Characters.Player.Abilities.Chargeable;
using Characters.Player.Abilities.Config;
using Characters.Player.Abilities.Tickable;
using Characters.Player.Input;
using Characters.Player.States;
using Systems.Damage;
using UnityEngine;

namespace Characters.Player
{
    public sealed class PlayerController : CharacterController2D
    {
        [Header("Movement details")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] [Range(0, 1)] private float _inAirMoveMultiplier = 0.5f;
        [SerializeField] [Range(0, 1)] private float _wallSlideMultiplier = 0.8f;
        [SerializeField] private PlayerJumpAbilityConfig _jumpConfig;
        [SerializeField] private PlayerDashAbilityConfig _dashConfig;

        [Header("Attack details")]
        [SerializeField] private int _attacksCount = 3;
        [SerializeField] private Vector2[] _attackVelocities =
        {
            new(3f, 1.5f),
            new(1f, 2.5f),
            new(4f, 5f)
        };
        [SerializeField] private float _attackVelocityDuration = 0.1f;
        [SerializeField] private float _attackResetTime = 1f;
        [SerializeField] private DamageDefinition[] _attackDefinitions =
        {
            new(15, DamageSource.Player, new Vector2(4f, 0f)),
            new(15, DamageSource.Player, new Vector2(4f, 0f)),
            new(25, DamageSource.Player, new Vector2(7f, 3f))
        };
        [SerializeField] private Vector2 _jumpAttackVelocity = new(3f, -5f);
        [SerializeField] private DamageDefinition _jumpAttackDefinition = new(40, DamageSource.Player, new Vector2(3f, 0f), 0.2f);

        [SerializeField] private PlayerInputHandler _inputHandler;

        private PlayerAbilityContext _abilityContext;
        private Dictionary<PlayerAbilityId, IPlayerAbility> _abilities;
        private IPlayerTickableAbility[] _tickableAbilities;

        public float MoveSpeed => _moveSpeed;
        public float InAirMoveMultiplier => _inAirMoveMultiplier;
        public float WallSlideMultiplier => _wallSlideMultiplier;

        public int AttacksCount => _attacksCount;
        public Vector2[] AttackVelocities => _attackVelocities;
        public float AttackVelocityDuration => _attackVelocityDuration;
        public float AttackResetTime => _attackResetTime;
        public DamageDefinition[] AttackDefinitions => _attackDefinitions;
        public Vector2 JumpAttackVelocity => _jumpAttackVelocity;
        public DamageDefinition JumpAttackDefinition => _jumpAttackDefinition;

        public IPlayerInput Input => _inputHandler;

        public void ChangeState(PlayerStateId id)
        {
            ChangeState((int)id);
        }

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

        public void RefillChargeableAbility(PlayerAbilityId id)
        {
            if (TryGetChargeableAbility(id, out IPlayerChargeableAbility ability))
            {
                ability.Refill();
            }
        }

        public void RefillChargeableAbility(PlayerAbilityId id, int amount)
        {
            if (TryGetChargeableAbility(id, out IPlayerChargeableAbility ability))
            {
                ability.RefillTo(amount);
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

            RegisterState(new PlayerIdleState(this));
            RegisterState(new PlayerMoveState(this));
            RegisterState(new PlayerDashState(this, _dashConfig));
            RegisterState(new PlayerJumpState(this, _jumpConfig));
            RegisterState(new PlayerFallState(this));
            RegisterState(new PlayerWallSlideState(this));
            RegisterState(new PlayerWallJumpState(this, _jumpConfig));
            RegisterState(new PlayerAttackState(this));
            RegisterState(new PlayerJumpAttackState(this));
            RegisterState(new PlayerHurtState(this));
        }

        protected override void OnEnabled()
        {
            _inputHandler.Enable();
        }

        protected override void OnStarted()
        {
            StartStateMachine((int)PlayerStateId.Idle);
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

        protected override void OnDamageImpactReceived(DamageImpactInfo damageImpact)
        {
            // ChangeState(PlayerStateId.Hurt);
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

        private bool TryGetChargeableAbility(PlayerAbilityId id, out IPlayerChargeableAbility chargeableAbility)
        {
            if (TryGetAbility(id, out IPlayerAbility ability) && ability is IPlayerChargeableAbility chargeable)
            {
                chargeableAbility = chargeable;
                return true;
            }

            chargeableAbility = null;
            return false;
        }
    }
}
