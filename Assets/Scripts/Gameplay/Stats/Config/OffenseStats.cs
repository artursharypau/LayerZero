using System;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal sealed class OffenseStats
    {
        [SerializeField] private float _damage;

        [SerializeField] private float _criticalDamageBonus;
        [SerializeField] private float _criticalDamageChance;

        [SerializeField] private float _elementalDamage;
        [SerializeField] private float _elementalDamageChance;
        [SerializeField] private float _elementalDamageDuration;

        public float Damage => _damage;

        public float CriticalDamageBonus => _criticalDamageBonus;
        public float CriticalDamageChance => _criticalDamageChance;

        public float ElementalDamage => _elementalDamage;
        public float ElementalDamageChance => _elementalDamageChance;
        public float ElementalDamageDuration => _elementalDamageDuration;
    }
}
