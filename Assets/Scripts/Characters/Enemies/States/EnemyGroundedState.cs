using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(EnemyController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Perception.TargetAcquired += OnTargetAcquired;
        }

        public override void Exit()
        {
            base.Exit();

            Perception.TargetAcquired -= OnTargetAcquired;
        }

        private void OnTargetAcquired()
        {
            Perception.TargetAcquired -= OnTargetAcquired;
            ChangeTo<EnemyChaseState>();
        }
    }
}
