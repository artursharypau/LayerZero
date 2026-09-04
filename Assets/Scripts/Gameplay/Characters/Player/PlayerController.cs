using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Characters.Common.Abilities;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Characters.Player.Abilities;
using LayerZero.Gameplay.Characters.Player.Config;
using LayerZero.Gameplay.Characters.Player.Input;
using LayerZero.Gameplay.Characters.Player.States;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Stats;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(DamageReceiver))]
    [RequireComponent(typeof(CombatSystem))]
    [RequireComponent(typeof(StatsSystem))]
    internal sealed class PlayerController : Character2D
    {
        [SerializeField] private PlayerConfig _config;

        private PlayerInput _input;

        public PlayerConfig Config => _config;
        public IPlayerInput Input => _input;
        public AbilitySet Abilities { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _input = new PlayerInput(_config.Input);

            Abilities = new AbilitySet()
                .Add(PlayerAbilityId.Jump, new JumpAbility(_config.Jump, _input))
                .Add(PlayerAbilityId.Dash, new DashAbility(_config.Dash, _input, Movement));

            StateMachine.Register(new PlayerIdleState(this));
            StateMachine.Register(new PlayerMoveState(this));
            StateMachine.Register(new PlayerJumpState(this));
            StateMachine.Register(new PlayerFallState(this));
            StateMachine.Register(new PlayerWallSlideState(this));
            StateMachine.Register(new PlayerWallJumpState(this));
            StateMachine.Register(new PlayerDashState(this));
            StateMachine.Register(new PlayerAttackState(this));
            StateMachine.Register(new PlayerJumpAttackState(this));
            StateMachine.Register(new PlayerHurtState(this));
            StateMachine.Register(new PlayerDeadState(this));
            StateMachine.Register(new PlayerCounterattackState(this));
        }

        private void Start()
        {
            Abilities.Refill(PlayerAbilityId.Jump);
            StateMachine.Start(PlayerStateId.Idle);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _input.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _input.Disable();
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }

        protected override void OnDamageImpactReceived(DamageImpactInfo impact)
        {
            StateMachine.ChangeState(PlayerStateId.Hurt, impact, StateTransitionMode.Immediate);
        }

        protected override void OnDied()
        {
            StateMachine.ChangeState(PlayerStateId.Dead, StateTransitionMode.Immediate);
        }
    }
}
