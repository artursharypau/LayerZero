using Characters.Common;
using Characters.Common.States;
using Characters.Enemy.States;
using Systems.Combat;
using Systems.Damage;
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

            _targetDetector.Initialize(this);
        }

        public bool ShouldAttack()
        {
            return _combatSystem.HasTargets();
        }

        protected override void OnStarted()
        {
            StartStateMachine(StateId.Idle);
        }

        protected override void OnUpdated()
        {
            _targetDetector.Tick(Time.deltaTime);
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
