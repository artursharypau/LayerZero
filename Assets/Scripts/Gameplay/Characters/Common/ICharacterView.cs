namespace LayerZero.Gameplay.Characters.Common
{
    /// <summary>
    /// Marks a presentation component living inside a character hierarchy that needs
    /// container injection. Such components are not registered in the character scope,
    /// so <see cref="CharacterLifetimeScope"/> injects them explicitly on build.
    /// </summary>
    public interface ICharacterView
    {
    }
}
