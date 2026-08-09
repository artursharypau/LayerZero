using System;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    [Serializable]
    public sealed class PerceptionConfig
    {
        [SerializeField] [Min(0f)] private float _distance = 13f;
        [SerializeField] [Min(0f)] private float _scanInterval = 0.5f;

        [Tooltip("How long a target stays remembered after it leaves sight.")] [SerializeField] [Min(0f)]
        private float _alertDuration = 5f;

        [Tooltip("Leave empty to fall back to the project's Player layer.")] [SerializeField]
        private LayerMask _targetMask;

        [Tooltip("Leave empty to fall back to the project's Ground layer.")] [SerializeField]
        private LayerMask _blockerMask;

        public float Distance => _distance;
        public float ScanInterval => _scanInterval;
        public float AlertDuration => _alertDuration;
        public LayerMask TargetMask => _targetMask;
        public LayerMask BlockerMask => _blockerMask;
    }
}
