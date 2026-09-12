using System;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal sealed class MajorStats
    {
        [SerializeField] private float _strength;
        [SerializeField] private float _agility;
        [SerializeField] private float _intelligence;
        [SerializeField] private float _vitality;

        public float Strength => _strength;
        public float Agility => _agility;
        public float Intelligence => _intelligence;
        public float Vitality => _vitality;
    }
}
