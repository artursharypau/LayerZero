using Characters.Common;
using Characters.Enemy.States;
using Systems.Combat;
using Systems.Damage;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(CombatSystem))]
    public abstract class EnemyController : CharacterController2D
    {
        [Header("Movement details")]
        [SerializeField] private float _idleDuration = 2f;
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] [Range(0, 5)] private float _moveAnimMultiplier = 1f;

        [Header("Chase details")]
        [SerializeField] [Range(0, 5)] private float _chaseMoveSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 5)] private float _chaseMoveAnimMultiplier = 2f;

        [Header("Attack details")]
        [SerializeField] private EnemyTargetDetector _targetDetector;
        [SerializeField] private DamageDefinition _attackDefinition = new(10, DamageSource.Enemy, new Vector2(4f, 2f));

        public float IdleDuration => _idleDuration;
        public float MoveSpeed => _moveSpeed;
        public float MoveAnimMultiplier => _moveAnimMultiplier;

        public float ChaseMoveSpeedMultiplier => _chaseMoveSpeedMultiplier;
        public float ChaseMoveAnimMultiplier => _chaseMoveAnimMultiplier;

        public EnemyTargetDetector TargetDetector => _targetDetector;
        public DamageDefinition AttackDefinition => _attackDefinition;

        public void ChangeState(EnemyStateId id)
        {
            ChangeState((int)id);
        }

        public void ChangeState<TArg>(EnemyStateId id, TArg arg)
        {
            ChangeState((int)id, arg);
        }

        protected override void OnAwakened()
        {
            RegisterState(new EnemyIdleState(this));
            RegisterState(new EnemyPatrolState(this));
            RegisterState(new EnemyChaseState(this));
            RegisterState(new EnemyAttackState(this));
            RegisterState(new EnemyHurtState(this));

            _targetDetector.Initialize(Movement);
        }

        protected override void OnStarted()
        {
            StartStateMachine((int)EnemyStateId.Idle);
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

        protected override void OnDamageImpactReceived(DamageImpactInfo damageImpact)
        {
            ChangeState(EnemyStateId.Hurt, damageImpact);
        }
    }
}
