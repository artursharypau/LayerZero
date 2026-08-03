using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>Passive ground behaviour. Any of these states drops into chase as soon as a target appears.</summary>
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
