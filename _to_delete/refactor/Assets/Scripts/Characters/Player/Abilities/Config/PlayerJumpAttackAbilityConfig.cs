using System;
using Systems.Damage;
using UnityEngine;

namespace Characters.Player.Abilities.Config
{
    [Serializable]
    public class PlayerJumpAttackAbilityConfig
    {
        [SerializeField] public Vector2 Velocity = new(3f, -5f);
        [SerializeField] public DamageDefinition Definition = new(40, DamageSource.Player, new Vector2(3f, 0f), 0.2f);
    }
}
