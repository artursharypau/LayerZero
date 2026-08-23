using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies.Config
{
    [Serializable]
    internal sealed class EnemyChaseConfig
    {
        [SerializeField] [Range(0f, 5f)] private float _speedMultiplier = 2f;
        [SerializeField] [Range(0f, 5f)] private float _animationMultiplier = 2f;

        public float SpeedMultiplier => _speedMultiplier;
        public float AnimationMultiplier => _animationMultiplier;
    }
}
