using System;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal sealed class Stat
    {
        [SerializeField] private float _value;

        public float Value => _value;
    }
}
