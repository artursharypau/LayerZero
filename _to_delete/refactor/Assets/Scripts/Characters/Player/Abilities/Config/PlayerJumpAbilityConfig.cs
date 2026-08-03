using System;
using UnityEngine;

namespace Characters.Player.Abilities.Config
{
    [Serializable]
    public class PlayerJumpAbilityConfig
    {
        [SerializeField] public int Charges = 2;
        [SerializeField] public float Force = 13f;
        [SerializeField] public Vector2 WallJumpForce = new(6f, 12f);
        [SerializeField] public float WallJumpMoveLockDuration = 0.2f;
    }
}
