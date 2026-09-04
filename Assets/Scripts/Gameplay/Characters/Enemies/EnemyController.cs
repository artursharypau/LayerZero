using LayerZero.Core.Diagnostics;
using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Characters.Enemies.Config;
using LayerZero.Gameplay.Characters.Enemies.Perception;
using LayerZero.Gameplay.Characters.Enemies.States;
using LayerZero.Gameplay.Combat;
using LayerZero.Gameplay.Combat.Attack;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Stats;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies
{
    [RequireComponent(typeof(CharacterMovement2D))]
    [RequireComponent(typeof(DamageReceiver))]
    [RequireComponent(typeof(CombatSystem))]
    [RequireComponent(typeof(StatsSystem))]
    internal sealed class EnemyController : Character2D
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
            StateMachine.Register(new EnemyStunnedState(this));
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
            if (impact.StunDuration > 0f)
            {
                StateMachine.ChangeState(EnemyStateId.Stunned, impact, StateTransitionMode.Immediate);
            }

            Movement.ApplyKnockback(impact.Knockback);
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
