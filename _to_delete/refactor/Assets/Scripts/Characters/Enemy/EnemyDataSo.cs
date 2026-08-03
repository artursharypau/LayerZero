using Systems.Damage;
using UnityEngine;

namespace Characters.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "LayerZero/Enemy Data")]
    public class EnemyDataSo : ScriptableObject
    {
        [Header("Movement details")]
        [SerializeField] private float _idleDuration = 2f;
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] [Range(0, 5)] private float _moveAnimMultiplier = 1f;

        [Header("Chase details")]
        [SerializeField] [Range(0, 5)] private float _chaseMoveSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 5)] private float _chaseMoveAnimMultiplier = 2f;

        [Header("Attack details")]
        [SerializeField] private DamageDefinition _attackDefinition = new(10, DamageSource.Enemy, new Vector2(4f, 2f));

        public float IdleDuration => _idleDuration;
        public float MoveSpeed => _moveSpeed;
        public float MoveAnimMultiplier => _moveAnimMultiplier;

        public float ChaseMoveSpeedMultiplier => _chaseMoveSpeedMultiplier;
        public float ChaseMoveAnimMultiplier => _chaseMoveAnimMultiplier;

        public DamageDefinition AttackDefinition => _attackDefinition;
    }
}
