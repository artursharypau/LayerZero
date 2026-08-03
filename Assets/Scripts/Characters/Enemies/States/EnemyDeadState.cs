using LayerZero.Characters.Common.States;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>Terminal enemy state. Hook loot drops, score and despawn here.</summary>
    public sealed class EnemyDeadState : DeadStateBase<EnemyController>
    {
        public EnemyDeadState(EnemyController owner)
            : base(owner)
        {
        }
    }
}
