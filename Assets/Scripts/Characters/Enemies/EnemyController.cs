using LayerZero.Characters.Common;
using LayerZero.Characters.Enemies.Config;
using LayerZero.Characters.Enemies.Perception;
using LayerZero.Characters.Enemies.States;
using LayerZero.Combat.Attacks;
using LayerZero.Combat.Damage;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.StateMachine;
using UnityEngine;

namespace LayerZero.Characters.Enemies
{
    public sealed class EnemyController : Character2D
    {
        [Header("Data")]
        [SerializeField] private EnemyConfig _config;

        [Header("Scene references")]
        [SerializeField] private Transform _perceptionOrigin;

        public EnemyConfig Config => _config;
        public EnemyTargetPerception Perception { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Perception = new EnemyTargetPerception(_config.Perception, _perceptionOrigin, Movement);

            StateMachine.Register(new EnemyIdleState(this));
            StateMachine.Register(new EnemyPatrolState(this));
            StateMachine.Register(new EnemyHurtState(this));
            StateMachine.Register(new EnemyDeadState(this));

            RegisterCombatStates(_config.Attack.Kind);
        }

        private void Start()
        {
            StateMachine.Start(EnemyStateId.Idle);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Perception.FixedUpdate();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Perception?.ForgetTarget();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Perception?.DrawGizmos();
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

        private void RegisterCombatStates(AttackKind kind)
        {
            switch (kind)
            {
                case AttackKind.Melee:
                    StateMachine.Register(new EnemyChaseState(this));
                    StateMachine.Register(new EnemyMeleeAttackState(this));
                    break;
                default:
                    GameLog.Error(this, $"'{name}' has no combat states for attack kind '{kind}'.");
                    break;
            }
        }
    }
}
