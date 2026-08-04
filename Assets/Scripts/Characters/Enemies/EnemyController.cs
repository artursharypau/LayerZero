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
    public abstract class EnemyController : Character
    {
        [Header("Data")]
        [SerializeField] private EnemyConfig _config;

        [Header("Scene references")]
        [Tooltip("Where the line-of-sight ray starts from.")] [SerializeField]
        private Transform _sightOrigin;

        public EnemyConfig Config => _config;
        public TargetPerception Perception { get; private set; }

        protected override void OnInitialized()
        {
            if (!_config)
            {
                GameLog.Error(this, $"'{name}' has no {nameof(EnemyConfig)} assigned.");
                return;
            }

            Perception = new TargetPerception(Movement, _config.Perception, _sightOrigin);

            StateMachine.Register(new EnemyIdleState(this));
            StateMachine.Register(new EnemyPatrolState(this));
            StateMachine.Register(new EnemyHurtState(this));
            StateMachine.Register(new EnemyDeadState(this));

            RegisterCombatBehaviour();
        }

        protected abstract void RegisterCombatBehaviour();

        protected override void OnStarted()
        {
            if (_config)
            {
                StateMachine.Start(EnemyStateId.Idle);
            }
        }

        protected override void OnFixedUpdated(float deltaTime)
        {
            Perception.FixedTick(deltaTime);
        }

        protected override void OnDisabled()
        {
            Perception.Disable();
        }

        protected override void OnGizmosDrawn()
        {
            Perception.DrawGizmos();
        }

        protected override void OnDamaged(DamageInfo damageInfo)
        {
            Perception.NotifyDamaged(damageInfo);
        }

        protected override void OnDamageImpactReceived(DamageImpactInfo impact)
        {
            StateMachine.ChangeState(EnemyStateId.Hurt, impact, StateTransitionMode.Immediate);
        }

        protected override void OnDied()
        {
            StateMachine.ChangeState(EnemyStateId.Dead, StateTransitionMode.Immediate);
        }
    }
}
