using Characters.Common;
using Characters.Enemy.States;
using Infrastructure.StateMachine;
using Systems.Combat;
using UnityEngine;

namespace Characters.Enemy
{
    public abstract class EnemyController : CharacterControllerBase
    {
        [Header("Movement details")]
        [SerializeField] private float _idleDuration = 2f;
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] [Range(0, 5)] private float _moveAnimMultiplier = 1f;

        [Header("Chase details")]
        [SerializeField] [Range(0, 5)] private float _chaseMoveSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 5)] private float _chaseMoveAnimMultiplier = 2f;

        [SerializeField] private EnemyTargetDetector _targetDetector;

        private CombatSystem _combatSystem;

        public float IdleDuration => _idleDuration;
        public float MoveSpeed => _moveSpeed;
        public float MoveAnimMultiplier => _moveAnimMultiplier;

        public float ChaseMoveSpeedMultiplier => _chaseMoveSpeedMultiplier;
        public float ChaseMoveAnimMultiplier => _chaseMoveAnimMultiplier;

        public EnemyTargetDetector TargetDetector => _targetDetector;

        protected override void OnAwakened()
        {
            _combatSystem = GetComponent<CombatSystem>();

            RegisterState(StateId.Idle, new EnemyIdleState(this));
            RegisterState(StateId.Patrol, new EnemyPatrolState(this));
            RegisterState(StateId.Chase, new EnemyChaseState(this));
            RegisterState(StateId.Attack, new EnemyAttackState(this));

            _targetDetector.Initialize(this, this);
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();

            Health.Damaged += OnDamaged;
        }

        protected override void OnStarted()
        {
            StartStateMachine(StateId.Idle);
        }

        protected override void OnUpdated()
        {
            _targetDetector.Tick(Time.deltaTime);
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();

            Health.Damaged -= OnDamaged;
        }

        protected override void OnGizmosDrawn()
        {
            _targetDetector.DrawGizmos();
        }

        public bool ShouldAttack()
        {
            return _combatSystem.HasTargets();
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            TargetDetector.DamageAlert(damageInfo);
        }
    }
}
