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

        public State IdleState { get; private set; }
        public State PatrolState { get; private set; }
        public State AttackState { get; private set; }
        public State ChaseState { get; private set; }

        protected override void OnAwakened()
        {
            _combatSystem = GetComponent<CombatSystem>();

            IdleState = new EnemyIdleState(FSM, this);
            PatrolState = new EnemyPatrolState(FSM, this);
            AttackState = new EnemyAttackState(FSM, this);
            ChaseState = new EnemyChaseState(FSM, this);

            _targetDetector.Initialize(this);
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();

            Health.Damaged += OnDamaged;
        }

        protected override void OnStarted()
        {
            FSM.Initialize(IdleState);
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

        protected override void OnDrownGizmos()
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
