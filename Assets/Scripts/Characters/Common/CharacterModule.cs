namespace LayerZero.Characters.Common
{
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
