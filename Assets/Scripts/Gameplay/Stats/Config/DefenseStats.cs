using System;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal sealed class DefenseStats
    {
        [SerializeField] private float _armor;
        [SerializeField] private float _evasionChance;
        [SerializeField] private float _elementalResistance;

        public float Armor => _armor;
        public float EvasionChance => _evasionChance;
        public float ElementalResistance => _elementalResistance;
    }
}
