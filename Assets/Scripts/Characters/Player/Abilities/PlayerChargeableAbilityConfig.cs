using System;
using UnityEngine;

namespace Characters.Player.Abilities
{
    [Serializable]
    public class PlayerChargeableAbilityConfig : IPlayerAbilityConfig
    {
        [SerializeField] public ushort Charges = 2;
    }
}
