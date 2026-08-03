using System;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    [Serializable]
    public sealed class RangedCombatSettings
    {
        [Tooltip("Closer than this and the enemy backs off instead of shooting.")]
        [SerializeField] [Min(0f)] private float _preferredDistance = 7f;

        [Tooltip("Tolerance around the preferred distance, so the enemy does not jitter.")]
        [SerializeField] [Min(0f)] private float _distanceTolerance = 1.5f;

        [SerializeField] [Min(0f)] private float _recoveryDuration = 0.75f;
        [SerializeField] [Range(0f, 5f)] private float _retreatSpeedMultiplier = 1.5f;

        public float PreferredDistance => _preferredDistance;
        public float DistanceTolerance => _distanceTolerance;

        /// <summary>Cooldown between shots, spent standing still.</summary>
        public float RecoveryDuration => _recoveryDuration;

        public float RetreatSpeedMultiplier => _retreatSpeedMultiplier;
    }
}
