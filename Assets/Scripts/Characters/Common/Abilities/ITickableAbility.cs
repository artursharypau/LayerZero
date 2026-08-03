using LayerZero.Core.Timing;

namespace LayerZero.Characters.Common.Abilities
{
    /// <summary>An ability with its own timeline (cooldown, wind-up, duration).</summary>
    public interface ITickableAbility : IAbility, ITickable
    {
    }
}
