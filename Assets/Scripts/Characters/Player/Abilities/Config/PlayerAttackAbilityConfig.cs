using System;
using Systems.Damage;
using UnityEngine;

namespace Characters.Player.Abilities.Config
{
    [Serializable]
    public class PlayerAttackAbilityConfig
    {
        [SerializeField] public int AttacksCount = 3;
        [SerializeField] public Vector2[] AttackVelocities =
        {
            new(3f, 1.5f),
            new(1f, 2.5f),
            new(4f, 5f)
        };
        [SerializeField] public float AttackVelocityDuration = 0.1f;
        [SerializeField] public float AttackResetTime = 1f;
        [SerializeField] public DamageDefinition[] AttackDefinitions =
        {
            new(15, DamageSource.Player, new Vector2(4f, 0f)),
            new(15, DamageSource.Player, new Vector2(4f, 0f)),
            new(25, DamageSource.Player, new Vector2(7f, 3f))
        };
    }
}
