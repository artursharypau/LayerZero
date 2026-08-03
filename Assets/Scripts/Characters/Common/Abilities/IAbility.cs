namespace LayerZero.Characters.Common.Abilities
{
    /// <summary>
    /// A resource-gated action a character can perform (jump, dash, parry).
    /// An ability owns its availability rules; states only ask and trigger.
    /// </summary>
    public interface IAbility
    {
        bool CanUse();
        void Use();
    }
}
