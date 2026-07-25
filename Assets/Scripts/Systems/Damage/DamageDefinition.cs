using System;
using UnityEngine;

namespace Systems.Damage
{
    [Serializable]
    public class DamageDefinition
    {
        [SerializeField] private int _amount;
        [SerializeField] private DamageSource _source;

        [Header("Impact")]
        [SerializeField] private Vector2 _knockback;
        [SerializeField] private float _stunDuration;

        public int Amount => _amount;
        public DamageSource Source => _source;
        public Vector2 Knockback => _knockback;
        public float StunDuration => _stunDuration;
    }
}
