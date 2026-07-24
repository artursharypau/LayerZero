using System;
using UnityEngine;

namespace Characters.Player.Abilities.Jump
{
    [Serializable]
    public class PlayerJumpAbilityConfig : IPlayerAbilityConfig
    {
        [SerializeField] public float Force = 13f;
        [SerializeField] public Vector2 WallJumpForce = new(6f, 12f);
        [SerializeField] public float WallJumpMoveLockDuration = 0.2f;
    }
}
