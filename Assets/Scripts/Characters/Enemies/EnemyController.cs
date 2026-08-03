using LayerZero.Characters.Common;
using LayerZero.Characters.Enemies.Config;
using LayerZero.Characters.Enemies.Perception;
using LayerZero.Characters.Enemies.States;
using LayerZero.Combat.Damage;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.StateMachine;
using UnityEngine;

namespace LayerZero.Characters.Enemies
{
    /// <summary>
    /// Everything every enemy shares: senses, idle/patrol wandering, the hurt reaction and death.
    /// <para>
    /// It deliberately knows nothing about how the enemy fights. An archetype supplies its combat
    /// behaviour in <see cref="RegisterCombatBehaviour" /> - that single seam is what keeps this
    /// class from growing a branch per enemy type.
    /// </para>
    /// </summary>
    public abstract class EnemyController : Character
    {
        [Header("Data")]
        [SerializeField] private EnemyConfig _config;

        [Header("Scene references")]
        [Tooltip("Where the line-of-sight ray starts from.")]
        [SerializeField] private Transform _sightOrigin;

        public EnemyConfig Config => _config;
        public TargetPerception Perception { get; private set; }

        protected override void Compose()
        {
            if (!_config)
            {
                GameLog.Error(this, $"'{name}' has no {nameof(EnemyConfig)} assigned.");
                return;
            }

            Perception = AddModule(new TargetPerception(_config.Perception, _sightOrigin));

            States.Register(new EnemyIdleState(this));
            States.Register(new EnemyPatrolState(this));
            States.Register(new EnemyHurtState(this));
            States.Register(new EnemyDeadState(this));

            RegisterCombatBehaviour();
        }

        /// <summary>
        /// Registers the chase and attack states that define this archetype.
        /// Melee registers a straight chase and a swing; ranged registers a kiting chase and a shot.
        /// </summary>
        protected abstract void RegisterCombatBehaviour();

        protected override void OnStarted()
        {
            if (_config)
            {
                States.Start<EnemyIdleState>();
            }
        }

        protected override void OnDamaged(DamageInfo damageInfo)
        {
            Perception?.NotifyDamaged(damageInfo);
        }

        protected override void OnImpactReceived(DamageImpactInfo impact)
        {
            States.ChangeState<EnemyHurtState, DamageImpactInfo>(impact, StateTransitionMode.Immediate);
        }

        protected override void OnDied()
        {
            States.ChangeState<EnemyDeadState>(StateTransitionMode.Immediate);
        }

        /// <summary>Typed access to the config for archetypes that need their own settings section.</summary>
        protected TConfig RequireConfig<TConfig>() where TConfig : EnemyConfig
        {
            if (_config is TConfig typed)
            {
                return typed;
            }

            GameLog.Error(
                this,
                $"'{name}' needs a '{typeof(TConfig).Name}' asset but got '{(_config ? _config.GetType().Name : "none")}'.");

            return null;
        }
    }
}
