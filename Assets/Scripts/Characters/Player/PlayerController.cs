using LayerZero.Characters.Common;
using LayerZero.Characters.Common.Abilities;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;
using LayerZero.Characters.Player.States;
using LayerZero.Combat.Damage;
using LayerZero.Core.StateMachine;
using UnityEngine;

namespace LayerZero.Characters.Player
{
    /// <summary>
    /// The player's composition root. It declares which modules and states the player has and
    /// nothing else - no tuning fields, no per-frame logic, no combat rules.
    /// </summary>
    public sealed class PlayerController : Character
    {
        [SerializeField] private PlayerConfig _config;

        private PlayerInputModule _input;

        public PlayerConfig Config => _config;
        public IPlayerInput Input => _input;
        public AbilitySet<PlayerAbilityId> Abilities { get; private set; }

        protected override void Compose()
        {
            _input = AddModule(new PlayerInputModule(_config.Input));

            Abilities = AddModule(new AbilitySet<PlayerAbilityId>()
                .Add(PlayerAbilityId.Jump, new JumpAbility(_config.Jump, _input))
                .Add(PlayerAbilityId.Dash, new DashAbility(_config.Dash, _input, Movement)));

            States.Register(new PlayerIdleState(this));
            States.Register(new PlayerMoveState(this));
            States.Register(new PlayerJumpState(this));
            States.Register(new PlayerFallState(this));
            States.Register(new PlayerWallSlideState(this));
            States.Register(new PlayerWallJumpState(this));
            States.Register(new PlayerDashState(this));
            States.Register(new PlayerAttackState(this));
            States.Register(new PlayerJumpAttackState(this));
            States.Register(new PlayerHurtState(this));
            States.Register(new PlayerDeadState(this));
        }

        protected override void OnStarted()
        {
            Abilities.Refill(PlayerAbilityId.Jump);
            States.Start<PlayerIdleState>();
        }

        protected override void OnImpactReceived(DamageImpactInfo impact)
        {
            States.ChangeState<PlayerHurtState, DamageImpactInfo>(impact, StateTransitionMode.Immediate);
        }

        protected override void OnDied()
        {
            States.ChangeState<PlayerDeadState>(StateTransitionMode.Immediate);
        }
    }
}
