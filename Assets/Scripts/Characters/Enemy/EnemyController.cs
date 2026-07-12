using Characters.Enemy.States;
using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Enemy
{
    public abstract class EnemyController : CharacterController
    {
        [Header("Movement details")]
        [SerializeField] private float _idleDuration = 2f;
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] [Range(0, 5)] private float _moveAnimMultiplier = 1f;

        [Header("Player detection")]
        [SerializeField] private Transform _playerCheckPoint;
        [SerializeField] private float _playerCheckDistance = 13f;

        [Header("Battle details")]
        [SerializeField] [Range(0, 5)] private float _battleMoveSpeedMultiplier = 2f;
        [SerializeField] private float _attackDistance = 2.5f;
        [SerializeField] [Range(0, 5)] private float _battleMoveAnimMultiplier = 2f;
        [SerializeField] private float _inBattleChaseDuration = 5f;

        public float IdleDuration => _idleDuration;
        public float MoveSpeed => _moveSpeed;
        public float MoveAnimMultiplier => _moveAnimMultiplier;

        public float BattleMoveSpeedMultiplier => _battleMoveSpeedMultiplier;
        public float AttackDistance => _attackDistance;
        public float BattleMoveAnimMultiplier => _battleMoveAnimMultiplier;
        public float InBattleChaseDuration => _inBattleChaseDuration;

        public bool IsPlayerDetected => CheckForPlayer();

        public State IdleState { get; private set; }
        public State MoveState { get; private set; }
        public State AttackState { get; private set; }
        public State BattleState { get; private set; }

        protected override void OnAwake()
        {
            IdleState = new EnemyIdleState(FSM, this);
            MoveState = new EnemyMoveState(FSM, this);
            AttackState = new EnemyAttackState(FSM, this);
            BattleState = new EnemyBattleState(FSM, this);
        }

        protected override void OnStart()
        {
            FSM.Initialize(IdleState);
        }

        protected override void OnDrawAdditionalGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                _playerCheckPoint.position,
                _playerCheckPoint.position + new Vector3(_playerCheckDistance * FacingDirection, 0f));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_playerCheckPoint.position, _playerCheckPoint.position + new Vector3(_attackDistance * FacingDirection, 0f));
        }

        public RaycastHit2D CheckForPlayer()
        {
            RaycastHit2D raycast = Physics2D.Raycast(
                _playerCheckPoint.position,
                Vector2.right * FacingDirection,
                _playerCheckDistance,
                LayerMaskProvider.Player | LayerMaskProvider.Ground);

            return raycast.collider && LayerMaskProvider.Contains(raycast.collider.gameObject.layer, LayerMaskProvider.Player)
                ? raycast
                : default;
        }
    }
}
