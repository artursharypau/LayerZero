using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Enemies.Perception;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyHurtState : HurtStateBase<EnemyController>
    {
        public EnemyHurtState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Hurt)
        {
        }

        public override int Id => EnemyStateId.Hurt;

        private TargetPerception Perception => Owner.Perception;

        protected override void OnHurtFinished()
        {
            if (Perception.HasTarget)
            {
                ChangeTo(EnemyStateId.Chase);
                return;
            }

            ChangeTo(EnemyStateId.Patrol);
        }
    }
}
