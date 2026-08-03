using System;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    [Serializable]
    public sealed class PerceptionSettings
    {
        [SerializeField] [Min(0f)] private float _sightDistance = 13f;
        [SerializeField] [Min(0f)] private float _scanInterval = 0.5f;

        [Tooltip("How long a target stays remembered after it leaves sight.")]
        [SerializeField] [Min(0f)] private float _alertDuration = 5f;

        [Tooltip("Leave empty to fall back to the project's Player layer.")]
        [SerializeField] private LayerMask _targetMask;

        [Tooltip("Leave empty to fall back to the project's Ground layer.")]
        [SerializeField] private LayerMask _blockerMask;

        public float SightDistance => _sightDistance;
        public float ScanInterval => _scanInterval;
        public float AlertDuration => _alertDuration;
        public LayerMask TargetMask => _targetMask;
        public LayerMask BlockerMask => _blockerMask;
    }
}
