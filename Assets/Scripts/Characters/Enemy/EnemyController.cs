using Characters.Common;
using Characters.Enemy.States;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Enemy
{
    public abstract class EnemyController : CharacterControllerBase<EnemyStateId>
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

            RegisterState(EnemyStateId.Idle, new EnemyIdleState(this));
            RegisterState(EnemyStateId.Patrol, new EnemyPatrolState(this));
            RegisterState(EnemyStateId.Chase, new EnemyChaseState(this));
            RegisterState(EnemyStateId.Attack, new EnemyAttackState(this));

            _targetDetector.Initialize(Movement);
        }

        public bool ShouldAttack()
        {
            return _combatSystem.IsInRange(TargetDetector.Current);
        }

        protected override void OnStarted()
        {
            StartStateMachine(EnemyStateId.Idle);
        }

        protected override void OnFixedUpdated()
        {
            _targetDetector.Tick(Time.fixedDeltaTime);
        }

        protected override void OnGizmosDrawn()
        {
            _targetDetector.DrawGizmos();
        }

        protected override void OnDamaged(DamageInfo damageInfo)
        {
            TargetDetector.DamageAlert(damageInfo);
        }
    }
}
