using System;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    [Serializable]
    public sealed class EnemyMovementConfig
    {
        [SerializeField] [Min(0f)] private float _idleDuration = 2f;
        [SerializeField] [Min(0f)] private float _moveSpeed = 1.5f;
        [SerializeField] [Range(0f, 5f)] private float _moveAnimationMultiplier = 1f;

        public float IdleDuration => _idleDuration;
        public float MoveSpeed => _moveSpeed;
        public float MoveAnimationMultiplier => _moveAnimationMultiplier;
    }
}
