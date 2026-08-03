using LayerZero.Characters.Common.States;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyDeadState : DeadStateBase<EnemyController>
    {
        public EnemyDeadState(EnemyController owner)
            : base(owner)
        {
        }
    }
}
