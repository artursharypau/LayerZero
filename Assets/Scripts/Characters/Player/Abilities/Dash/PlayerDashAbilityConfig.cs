using System;
using UnityEngine;

namespace Characters.Player.Abilities.Dash
{
    [Serializable]
    public class PlayerDashAbilityConfig : IPlayerAbilityConfig
    {
        [SerializeField] public float Duration = 0.2f;
        [SerializeField] [Range(1, 5)] public float SpeedMultiplier = 3f;
        [SerializeField] public float Cooldown = 2f;
    }
}
