namespace LayerZero.Characters.Common
{
    /// <summary>
    /// A self-contained slice of character behaviour that is not a state: input, abilities,
    /// perception, stamina, buffs...
    /// <para>
    /// Modules are how a controller stays thin - it only declares which modules a character has,
    /// it does not implement them. A new enemy archetype adds a module instead of adding fields
    /// and Update() branches to a shared controller.
    /// </para>
    /// </summary>
    public abstract class CharacterModule
    {
        protected Character Owner { get; private set; }

        internal void Bind(Character owner)
        {
            Owner = owner;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        public virtual void Enable()
        {
        }

        public virtual void Disable()
        {
        }

        public virtual void Tick(float deltaTime)
        {
        }

        public virtual void FixedTick(float deltaTime)
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void DrawGizmos()
        {
        }
    }
}
